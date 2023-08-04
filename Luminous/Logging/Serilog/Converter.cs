using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace LangM.AspNetCore
{
    /// <summary>
    ///     配置转换
    /// </summary>
    public static class Converter
    {
        /// <summary>
        ///     转换日志等级
        /// </summary>
        public static LogEventLevel Level(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return LogEventLevel.Verbose;
                case LogLevel.Debug:
                    return LogEventLevel.Debug;
                case LogLevel.Information:
                    return LogEventLevel.Information;
                case LogLevel.Warning:
                    return LogEventLevel.Warning;
                case LogLevel.Error:
                    return LogEventLevel.Error;
                case LogLevel.Critical:
                    return LogEventLevel.Fatal;
                default:
                    return LogEventLevel.Verbose;
            }
        }

        /// <summary>
        ///     转换文件切割时间
        /// </summary>
        public static RollingInterval Interval(FileInterval interval)
        {
            switch (interval)
            {
                case FileInterval.Year:
                    return RollingInterval.Year;
                case FileInterval.Month:
                    return RollingInterval.Month;
                case FileInterval.Day:
                    return RollingInterval.Day;
                case FileInterval.Hour:
                    return RollingInterval.Hour;
                case FileInterval.Minute:
                    return RollingInterval.Minute;
                default:
                    return RollingInterval.Day;
            }
        }
    }
}