using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Luminous
{
    /// <summary>
    ///     Serilog
    /// </summary>
    public class SerilogLoggingFramework : ILoggingFramework
    {
        /// <summary>
        ///     配置日志(可以同时配置文件、mysql、elasticsearch)
        /// </summary>
        /// <param name="builder"><see cref="IHostBuilder"/></param>
        /// <param name="options">日志选项</param>
        public IHostBuilder Configure(IHostBuilder builder, LoggingOptions options)
        {
            ArgumentChecker.ThrowIfNull(builder, nameof(builder));
            ArgumentChecker.ThrowIfNull(options, nameof(options));

            var configuration = CreateConfiguration(options.MinLevel);

            if (options.File != null)
            {
                configuration.Configure(options.File, options.MinLevel);
            }

            if (options.Mysql != null)
            {
                configuration.Configure(options.Mysql, options.MinLevel);
            }

            if (options.SqlServer != null)
            {
                configuration.Configure(options.SqlServer, options.MinLevel);
            }

            if (options.ElasticSearch != null)
            {
                configuration.Configure(options.ElasticSearch, options.MinLevel);
            }

            Log.Logger = configuration.CreateLogger();
            return builder.UseSerilog(Log.Logger);
        }

        /// <summary>
        ///    配置文件日志
        /// </summary>
        /// <param name="builder"><see cref="IHostBuilder"/></param>
        /// <param name="options">文件日志选项</param>
        public IHostBuilder Configure(IHostBuilder builder, FileLoggingOptions options, LogLevel level)
        {
            ArgumentChecker.ThrowIfNull(builder, nameof(builder));
            ArgumentChecker.ThrowIfNull(options, nameof(options));

            var configuration = CreateConfiguration(level);

            configuration.Configure(options, level);

            Log.Logger = configuration.CreateLogger();

            return builder.UseSerilog(Log.Logger);
        }

        /// <summary>
        ///     配置 mysql 日志
        /// </summary>
        /// <param name="builder"><see cref="IHostBuilder"/></param>
        /// <param name="options">mysql 日志选项</param>
        public IHostBuilder Configure(IHostBuilder builder, MySqlLoggingOptions options, LogLevel level)
        {
            ArgumentChecker.ThrowIfNull(builder, nameof(builder));
            ArgumentChecker.ThrowIfNull(options, nameof(options));

            var configuration = CreateConfiguration(level);

            configuration.Configure(options, level);

            Log.Logger = configuration.CreateLogger();

            return builder.UseSerilog(Log.Logger);
        }

        /// <summary>
        ///     配置 sqlserver 日志
        /// </summary>
        /// <param name="builder"><see cref="IHostBuilder"/></param>
        /// <param name="options">sqlserver 日志选项</param>
        public IHostBuilder Configure(IHostBuilder builder, SqlServerLoggingOptions options, LogLevel level)
        {
            ArgumentChecker.ThrowIfNull(builder, nameof(builder));
            ArgumentChecker.ThrowIfNull(options, nameof(options));

            var configuration = CreateConfiguration(level);

            configuration.Configure(options, level);

            Log.Logger = configuration.CreateLogger();

            return builder.UseSerilog(Log.Logger);
        }

        /// <summary>
        ///     配置 elasticsearch 日志
        /// </summary>
        /// <param name="builder"><see cref="IHostBuilder"/></param>
        /// <param name="options">elasticsearch 日志选项</param>
        public IHostBuilder Configure(IHostBuilder builder, ElasticSearchLoggingOptions options, LogLevel level)
        {
            ArgumentChecker.ThrowIfNull(builder, nameof(builder));
            ArgumentChecker.ThrowIfNull(options, nameof(options));

            var configuration = CreateConfiguration(level);

            configuration.Configure(options, level);

            Log.Logger = configuration.CreateLogger();

            return builder.UseSerilog(Log.Logger);
        }

        /// <summary> 
        ///     初始化一个 serilog 配置
        /// </summary>
        /// <param name="fields">           日志需要记录的字段</param>
        /// <param name="minLevel">         日志最小等级</param>
        /// <param name="microsoftMinLevel">日志最小等级，仅对 Microsoft. 命名空间有效</param>
        private static LoggerConfiguration CreateConfiguration(LogLevel level)
        {
            var configuration = new LoggerConfiguration();

            var enrich = configuration.Enrich;

            enrich.FromLogContext();

            BindFields(enrich);

            configuration.Filter.With(new StreamlineUsefulFilter());

            SetLoggingLevel(configuration, Converter.Level(level));

            configuration.WriteTo.Console();

            return configuration;
        }

        /// <summary>
        ///     绑定日志需要显示的字段
        /// </summary>
        private static void BindFields(LoggerEnrichmentConfiguration enrich)
        {
            enrich.WithClientIp();

            enrich.WithClientAgent();

            enrich.WithThreadId();

            enrich.WithAccountUniqueId();

            enrich.WithUserName();

            enrich.WithEnvironmentName();

            enrich.WithQueryString();

            enrich.WithMachineName();
        }

        /// <summary>
        ///     设置日志等级
        /// </summary>
        private static void SetLoggingLevel(LoggerConfiguration configuration, LogEventLevel logLevel)
        {
            switch (logLevel)
            {
                case LogEventLevel.Verbose:
                    configuration.MinimumLevel.Verbose();
                    break;
                case LogEventLevel.Debug:
                    configuration.MinimumLevel.Debug();
                    break;
                case LogEventLevel.Information:
                    configuration.MinimumLevel.Information();
                    break;
                case LogEventLevel.Warning:
                    configuration.MinimumLevel.Warning();
                    break;
                case LogEventLevel.Error:
                    configuration.MinimumLevel.Error();
                    break;
                case LogEventLevel.Fatal:
                    configuration.MinimumLevel.Fatal();
                    break;
                default:
                    configuration.MinimumLevel.Verbose();
                    break;
            }
        }
    }
}