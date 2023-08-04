using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.MariaDB;
using Serilog.Sinks.MariaDB.Extensions;
using Serilog.Sinks.MSSqlServer;
using System.Data;

namespace Luminous
{
    internal static class LoggerConfigurationExtensions
    {
        /// <summary>
        ///     配置文件日志
        /// </summary>
        /// <param name="configuration"><see cref="LoggerConfiguration"/></param>
        /// <param name="options">文件日志选项</param>
        public static LoggerConfiguration Configure(this LoggerConfiguration configuration, FileLoggingOptions options, LogLevel level)
        {
            ArgumentChecker.ThrowIfNull(configuration, nameof(configuration));
            ArgumentChecker.ThrowIfNull(options, nameof(options));
            ArgumentChecker.ThrowIfNull(options.File, nameof(options.File));

            var logggingLevel = Converter.Level(level);
            var interval = Converter.Interval(options.Interval);

            configuration.WriteTo.File(Path.Combine(PathUtil.GetBinPath(), "langm-logs", options.File), rollingInterval: interval, restrictedToMinimumLevel: logggingLevel);

            return configuration;
        }

        /// <summary>
        ///     配置 mysql 日志
        /// </summary>
        /// <param name="configuration"><see cref="LoggerConfiguration"/></param>
        /// <param name="options">mysql 日志选项</param>
        public static LoggerConfiguration Configure(this LoggerConfiguration configuration, MySqlLoggingOptions options, LogLevel level)
        {
            ArgumentChecker.ThrowIfNull(configuration, nameof(configuration));
            ArgumentChecker.ThrowIfNull(options, nameof(options));
            ArgumentChecker.ThrowIfNull(options.ConnectionString, nameof(options.ConnectionString));
            ArgumentChecker.ThrowIfNull(options.Table, nameof(options.Table));

            var logggingLevel = Converter.Level(level);

            var mariadb = new MariaDBSinkOptions();
            var columns = mariadb.PropertiesToColumnsMapping;

            columns.Add("ClientIp", "ClientIp");

            columns.Add("ThreadId", "ThreadId");

            columns.Add("MachineName", "MachineName");

            columns.Add("RequestPath", "RequestPath");

            columns.Add("EnvironmentName", "EnvironmentName");

            //  自定义的字段

            columns.Add("es_AccountUniqueId", "AccountUniqueId");

            columns.Add("es_ClientAgent", "ClientAgent");

            columns.Add("es_UserName", "UserName");

            columns.Add("es_QueryString", "QueryString");

            //  移除日志模板字段
            columns.Remove("MessageTemplate");
            //  移除 Properties 字段
            columns.Remove("Properties");
            mariadb.ExcludePropertiesWithDedicatedColumn = true;

            configuration.AuditTo.MariaDB(
                connectionString: options.ConnectionString,
                tableName: options.Table,
                autoCreateTable: true,
                options: mariadb,
                restrictedToMinimumLevel: logggingLevel);

            return configuration;
        }

        /// <summary>
        ///     配置 sqlserver 日志
        /// </summary>
        /// <param name="configuration"><see cref="LoggerConfiguration"/></param>
        /// <param name="options">sqlserver 日志选项</param>
        public static LoggerConfiguration Configure(this LoggerConfiguration configuration, SqlServerLoggingOptions options, LogLevel level)
        {
            ArgumentChecker.ThrowIfNull(configuration, nameof(configuration));
            ArgumentChecker.ThrowIfNull(options, nameof(options));
            ArgumentChecker.ThrowIfNull(options.ConnectionString, nameof(options.ConnectionString));
            ArgumentChecker.ThrowIfNull(options.Table, nameof(options.Table));

            var logggingLevel = Converter.Level(level);

            var sinkOptions = new MSSqlServerSinkOptions { TableName = options.Table, AutoCreateSqlTable = true };
            var columnsOptions = new ColumnOptions();
            //  移除日志模板字段
            columnsOptions.Store.Remove(StandardColumn.MessageTemplate);
            //  移除 Properties 字段
            columnsOptions.Store.Remove(StandardColumn.Properties);
            columnsOptions.Properties.ExcludeAdditionalProperties = true;

            var columns = new List<SqlColumn>();

            columns.Add(Create("ClientIp", "ClientIp", SqlDbType.NVarChar));

            columns.Add(Create("ThreadId", "ThreadId", SqlDbType.Int));

            columns.Add(Create("MachineName", "MachineName", SqlDbType.NVarChar));

            columns.Add(Create("EnvironmentName", "EnvironmentName", SqlDbType.NVarChar));

            columns.Add(Create("RequestPath", "RequestPath", SqlDbType.NVarChar));

            //  自定义字段

            columns.Add(Create("es_AccountUniqueId", "AccountUniqueId", SqlDbType.NVarChar));

            columns.Add(Create("es_ClientAgent", "ClientAgent", SqlDbType.NVarChar));

            columns.Add(Create("es_UserName", "UserName", SqlDbType.NVarChar));

            columns.Add(Create("es_QueryString", "QueryString", SqlDbType.NVarChar));

            columnsOptions.AdditionalColumns = columns;

            configuration.WriteTo.MSSqlServer(
                connectionString: options.ConnectionString,
                sinkOptions: sinkOptions,
                columnOptions: columnsOptions,
                restrictedToMinimumLevel: logggingLevel);

            return configuration;

            SqlColumn Create(string propertyName, string columnName, SqlDbType type)
            {
                return new SqlColumn
                {
                    DataType = type,
                    ColumnName = columnName,
                    PropertyName = propertyName,
                    AllowNull = true,
                };
            }
        }

        /// <summary>
        ///     配置 elasticsearch 日志
        /// </summary>
        /// <param name="configuration"><see cref="LoggerConfiguration"/></param>
        /// <param name="options">elasticsearch 日志选项</param>
        public static LoggerConfiguration Configure(this LoggerConfiguration configuration, ElasticSearchLoggingOptions options, LogLevel level)
        {
            ArgumentChecker.ThrowIfNull(configuration, nameof(configuration));
            ArgumentChecker.ThrowIfNull(options, nameof(options.Urls));
            ArgumentChecker.ThrowIfNull(options.IndexFormat, nameof(options.IndexFormat));

            configuration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(options.Urls.Select(_ => new Uri(_)))
            {
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                IndexFormat = options.IndexFormat
            });

            return configuration;
        }
    }
}