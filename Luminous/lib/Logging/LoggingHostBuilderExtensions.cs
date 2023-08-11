namespace Microsoft.Extensions.Hosting
{
    public static class LoggingHostBuilderExtensions
    {
        private static readonly ILoggingFramework LoggingFramework = new SerilogLoggingFramework();

        /// <summary>
        ///     添加日志
        /// </summary>
        public static IHostBuilder AddLogging(this WebApplicationBuilder builder, LoggingOptions? defaultOptions = null)
        {
            defaultOptions ??= builder.Configuration.GetSection("Luminous:Log").Get<LoggingOptions>();

            return LoggingFramework.Configure(builder.Host, defaultOptions ?? CONFIGS.Log);
        }
    }
}
