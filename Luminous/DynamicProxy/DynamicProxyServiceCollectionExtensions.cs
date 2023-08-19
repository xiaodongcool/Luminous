using AspectCore.Configuration;
using AspectCore.DynamicProxy;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Luminous
{
    public static class DynamicProxyServiceCollectionExtensions
    {
        public static void AddLuminousDynamicProxyInterface<TInterface, TProxy>(this WebApplicationBuilder builder) where TProxy : LuminousInterceptor
        {
            builder.AddLuminousDynamicProxyInterface<TInterface, TProxy>(ServiceLifetime.Scoped);
        }

        public static void AddLuminousDynamicProxyInterface<TInterface, TProxy>(this WebApplicationBuilder builder, ServiceLifetime serviceLifetime) where TProxy : LuminousInterceptor
        {
            var interfaceType = typeof(TInterface);

            if (!interfaceType.IsInterface)
            {
                throw new ArgumentException(nameof(interfaceType));
            }

            Debug.Assert(Global.Solution != null);

            var types = Global.Solution.GetDerivedInterface(interfaceType);

            builder.Host.ConfigureServices((context, service) =>
            {
                var a = context.HostingEnvironment.ContentRootPath;
                service.ConfigureDynamicProxy(configure =>
                {
                    configure.Interceptors.AddTyped<TProxy>(Predicates.Implement(interfaceType));
                });

                foreach (var type in types)
                {
                    service.Add(new ServiceDescriptor(type, factory =>
                    {
                        return factory.GetRequiredService<IProxyGenerator>().CreateInterfaceProxy(type);
                    }, serviceLifetime));
                }
            }).UseServiceProviderFactory(new AspectCoreDependencyServiceProviderFactory());
        }
    }
}
