using Microsoft.Extensions.Hosting;

namespace Luminous
{
    public static class LoggingHostBuilderExtensions
    {
        private static readonly ILoggingFramework LoggingFramework = new SerilogLoggingFramework();

        public static IHostBuilder AddLuminousLogging(this WebApplicationBuilder webApplication, LoggingOptions? options = null)
        {
            return LoggingFramework.Configure(webApplication.Host, GetLoggingOptions(options));
        }

        public static IHostBuilder AddLuminousLogging(this IHostBuilder hostBuilder, LoggingOptions? options = null)
        {
            return LoggingFramework.Configure(hostBuilder, GetLoggingOptions(options));
        }

        private static LoggingOptions? GetLoggingOptions(LoggingOptions? options)
        {
            return options ?? LuminousConfiguration.Log;
        }
    }
}
