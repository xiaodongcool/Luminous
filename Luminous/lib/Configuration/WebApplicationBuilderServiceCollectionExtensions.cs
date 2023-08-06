using Com.Ctrip.Framework.Apollo;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WebApplicationBuilderServiceCollectionExtensions
    {
        /// <summary>
        ///     添加阿波罗配置中心
        /// </summary>
        public static IConfiguration AddConfiguration(this WebApplicationBuilder builder)
        {
            try
            {
                builder.Configuration
                    .AddApollo((builder.Configuration as IConfigurationBuilder).Build().GetSection("apollo"))
                    .AddDefault();
            }
            catch
            {
                //  阿波罗配置中心初始化失败，使用 appsettings.json
            }

            var configuration = (builder.Configuration as IConfigurationBuilder).Build();
            CONFIGURATION.ConfigurationCenter = configuration;
            return configuration;
        }
    }
}
