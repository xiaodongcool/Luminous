using Microsoft.Extensions.DependencyInjection;
using Nacos.AspNetCore.V2;

namespace Luminous
{
    public static class NacosServiceCollectionExtensions
    {
        /// <summary>
        ///     添加Nacos
        /// </summary>
        public static void AddLuminousNacos(this IServiceCollection serviceCollection, NacosConfig? config = null)
        {
            config ??= LuminousMicroserviceConfiguration.Nacos;

            serviceCollection.AddNacosAspNet(x =>
            {
                x.ServiceName = config.ServiceName;
                x.ServerAddresses = config.ServerAddresses;
                x.Namespace = config.Namespace;
                x.RegisterEnabled = true;
                x.InstanceEnabled = true;
            });
        }
    }
}