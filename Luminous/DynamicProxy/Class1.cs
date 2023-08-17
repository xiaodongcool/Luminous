using AspectCore.Configuration;
using AspectCore.DependencyInjection;
using AspectCore.DynamicProxy;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Luminous.DynamicProxy
{
    public static class LuminousDynamicProxyServiceCollectionExtensions
    {
        public static void AddLuminousDynamicProxy<TInterface, TProxy>(this WebApplicationBuilder builder, ServiceLifetime serviceLifetime) where TProxy : LuminousInterceptor
        {
            var interfaceType = typeof(TInterface);

            if (!interfaceType.IsInterface)
            {
                throw new ArgumentException(nameof(interfaceType));
            }

            var types = TypeContainer.FindChildInterface(interfaceType);

            builder.Host.ConfigureServices((context, service) =>
            {
                service.ConfigureDynamicProxy(configure =>
                {
                    configure.Interceptors.AddTyped<TProxy>(Predicates.Implement(interfaceType));
                });

                foreach (var type in types)
                {
                    service.Add(new ServiceDescriptor(type, factory => factory.GetRequiredService<IProxyGenerator>().CreateInterfaceProxy(type), serviceLifetime));
                }

            }).UseServiceProviderFactory(new DependencyServiceProviderFactory());
        }
    }

    [NonAspect]
    public class DependencyServiceProviderFactory : IServiceProviderFactory<IServiceContext>
    {
        public IServiceContext CreateBuilder(IServiceCollection services)
        {
            return services.ToServiceContext();
        }

        public IServiceProvider CreateServiceProvider(IServiceContext contextBuilder)
        {
            var serviceProvider = contextBuilder.Build();
            return serviceProvider;
        }
    }

    public abstract class LuminousInterceptor : IInterceptor
    {
        public bool AllowMultiple { get; }

        public bool Inherited { get; set; }

        public int Order { get; set; }

        public LuminousInterceptor()
        {
            Console.WriteLine(GetHashCode());
        }

        public abstract Task Invoke(AspectContext context, AspectDelegate next);

        protected virtual T? GetCustomerAttributeOnMethod<T>(AspectContext context) where T : Attribute
        {
            return context.ServiceMethod.GetCustomAttribute<T>();
        }

        protected virtual T? GetCustomerAttributeOnInterface<T>(AspectContext context) where T : Attribute
        {
            return context.ServiceMethod.DeclaringType?.GetCustomAttribute<T>();
        }

        protected Type GetMethodReturnType(AspectContext context)
        {
            return context.ServiceMethod.ReturnType;
        }

        protected ParameterInfo[] GetMethodParameters(AspectContext context)
        {
            return context.ServiceMethod.GetParameters();
        }

        protected object[] GetMethodParameterValues(AspectContext context)
        {
            return context.Parameters;
        }
    }
}
