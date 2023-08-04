using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;

namespace LangM.AspNetCore
{
    /// <summary>
    ///     日志选项
    /// </summary>
    public class LoggingOptions
    {
        /// <summary>
        ///     日志最小等级
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public LogLevel MinLevel { get; set; } = LogLevel.Trace;

        /// <summary>
        ///     文件日志选项
        /// </summary>
        public FileLoggingOptions File { get; set; }

        /// <summary>
        /// /   mysql 日志选项
        /// </summary>
        public MySqlLoggingOptions Mysql { get; set; }

        /// <summary>
        /// /   sqlserver 日志选项
        /// </summary>
        public SqlServerLoggingOptions SqlServer { get; set; }

        /// <summary>
        ///     elasticsearch 日志选项
        /// </summary>
        public ElasticSearchLoggingOptions ElasticSearch { get; set; }
    }
}