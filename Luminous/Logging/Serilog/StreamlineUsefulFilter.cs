using Serilog.Core;
using Serilog.Events;

namespace Luminous
{
    /// <summary>
    ///     精简有用的日志记录
    /// </summary>
    public class StreamlineUsefulFilter : ILogEventFilter
    {
        public StreamlineUsefulFilter() { }

        public bool IsEnabled(LogEvent logEvent)
        {
            var namespaces = logEvent.Properties["SourceContext"].ToString().Trim('"');

            if (namespaces.StartsWith("Microsoft."))
            {
                return logEvent.Level >= LogEventLevel.Warning;
            }
            else
            {
                return true;
            }
        }
    }
}
