using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Luminous
{
    /// <summary>
    ///     第三方日志框架
    /// </summary>
    public interface ILoggingFramework
    {
        /// <summary>
        ///     配置日志(可以同时配置文件、mysql、elasticsearch)
        /// </summary>
        /// <param name="builder"><see cref="IHostBuilder"/></param>
        /// <param name="options">日志选项</param>
        IHostBuilder Configure(IHostBuilder builder, LoggingOptions options);

        /// <summary>
        ///    配置文件日志
        /// </summary>
        /// <param name="builder"><see cref="IHostBuilder"/></param>
        /// <param name="options">文件日志配置</param>
        IHostBuilder Configure(IHostBuilder builder, FileLoggingOptions options, LogLevel level);

        /// <summary>
        ///     配置 mysql 日志
        /// </summary>
        /// <param name="builder"><see cref="IHostBuilder"/></param>
        /// <param name="options">mysql 日志选项</param>
        IHostBuilder Configure(IHostBuilder builder, MySqlLoggingOptions options, LogLevel level);

        /// <summary>
        ///     配置 sqlserver 日志
        /// </summary>
        /// <param name="builder"><see cref="IHostBuilder"/></param>
        /// <param name="options">sqlserver 日志选项</param>
        IHostBuilder Configure(IHostBuilder builder, SqlServerLoggingOptions options, LogLevel level);

        /// <summary>
        ///     配置 elasticsearch 日志
        /// </summary>
        /// <param name="builder"><see cref="IHostBuilder"/></param>
        /// <param name="options">elasticsearch 日志选项</param>
        IHostBuilder Configure(IHostBuilder builder, ElasticSearchLoggingOptions options, LogLevel level);
    }
}
