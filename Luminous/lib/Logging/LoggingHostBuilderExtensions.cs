﻿namespace Microsoft.Extensions.Hosting
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
            var a = options ?? Global.GetConfig<LoggingOptions>("Luminous:Log");
            return options ?? Global.GetConfig<LoggingOptions>("Luminous:Log");
        }
    }
}
