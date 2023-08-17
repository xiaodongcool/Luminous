using AspectCore.Configuration;
using AspectCore.DynamicProxy;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Luminous.DynamicProxy
{
    public static class LuminousDynamicProxyServiceCollectionExtensions
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

            var types = TypeContainer.FindChildInterface(interfaceType);

            builder.Host.ConfigureServices((context, service) =>
            {
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
            }).UseServiceProviderFactory(new DependencyServiceProviderFactory());
        }
    }
}
