using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting
{
    public static class LoggingHostBuilderExtensions
    {
        private static readonly ILoggingFramework LoggingFramework = new SerilogLoggingFramework();

        public static IHostBuilder AddLogging(this WebApplicationBuilder webApplication, LoggingOptions? defaultOptions = null)
        {
            return LoggingFramework.Configure(webApplication.Host, GetLoggingOptions(webApplication.Configuration, defaultOptions));
        }

        public static IHostBuilder AddLogging(this IHostBuilder hostBuilder, LoggingOptions? defaultOptions = null)
        {
            var configuration = hostBuilder.Build().Services.GetRequiredService<IConfiguration>();

            return LoggingFramework.Configure(hostBuilder, GetLoggingOptions(configuration, defaultOptions));
        }

        private static LoggingOptions GetLoggingOptions(IConfiguration configuration, LoggingOptions? defaultOptions)
        {
            return defaultOptions ?? configuration.GetSection("Luminous:Log").Get<LoggingOptions>() ?? CONFIGS.Log;
        }
    }
}
