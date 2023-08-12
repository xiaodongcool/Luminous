using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting
{
    public static class LoggingHostBuilderExtensions
    {
        private static readonly ILoggingFramework LoggingFramework = new SerilogLoggingFramework();

        public static IHostBuilder AddLogging(this WebApplicationBuilder webApplication, LoggingOptions? options = null)
        {
            return LoggingFramework.Configure(webApplication.Host, GetLoggingOptions(webApplication.Configuration, options));
        }

        public static IHostBuilder AddLogging(this IHostBuilder hostBuilder, LoggingOptions? options = null)
        {
            var configuration = hostBuilder.Build().Services.GetRequiredService<IConfiguration>();

            return LoggingFramework.Configure(hostBuilder, GetLoggingOptions(configuration, options));
        }

        private static LoggingOptions GetLoggingOptions(IConfiguration configuration, LoggingOptions? options)
        {
            return options ?? configuration.GetSection("Luminous:Log").Get<LoggingOptions>() ?? CONFIGS.Log;
        }
    }
}
