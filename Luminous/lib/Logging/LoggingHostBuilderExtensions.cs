using Microsoft.AspNetCore.Hosting;

namespace Microsoft.Extensions.Hosting
{
    public static class LoggingHostBuilderExtensions
    {
        private static readonly ILoggingFramework LoggingFramework = new SerilogLoggingFramework();

        /// <summary>
        ///     添加日志
        /// </summary>
        public static IHostBuilder AddLogging(this WebApplicationBuilder webApplication, LoggingOptions? defaultOptions = null)
        {
            defaultOptions ??= webApplication.Configuration.GetSection("Luminous:Log").Get<LoggingOptions>();

            return LoggingFramework.Configure(webApplication.Host, defaultOptions ?? CONFIGS.Log);
        }

        //public static IHostBuilder AddLogging(this IWebHostBuilder webHostBuilder, LoggingOptions? defaultOptions = null)
        //{
        //    defaultOptions ??= builder.Configuration.GetSection("Luminous:Log").Get<LoggingOptions>();

        //    return LoggingFramework.Configure(webHostBuilder, defaultOptions ?? CONFIGS.Log);
        //}
    }
}
