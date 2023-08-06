using Serilog.Core;
using Serilog.Events;

namespace Luminous
{
    /// <summary>
    ///     自定义额外的日志属性
    /// </summary>
    public abstract class CustomerLogEventEnricher : ILogEventEnricher
    {
        /// <summary>
        ///     属性名称
        /// </summary>
        protected abstract string PropertyName { get; }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            try
            {
                var propertyName = "es_" + PropertyName;

                var httpContext = new HttpContextAccessor().HttpContext;

                if (httpContext?.Items?[propertyName] is LogEventProperty property)
                {
                    logEvent.AddPropertyIfAbsent(property);
                }
                else
                {
                    var propertyValue = GetPropertyValue(logEvent, httpContext);

                    if (string.IsNullOrEmpty(propertyValue))
                    {
                        return;
                    }

                    property = new LogEventProperty(propertyName, new ScalarValue(propertyValue));
                    httpContext?.Items?.Add(propertyName, property);
                    logEvent.AddPropertyIfAbsent(property);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        /// <summary>
        ///     获取属性值
        /// </summary>
        protected abstract string GetPropertyValue(LogEvent logEvent, HttpContext httpContext);
    }
}