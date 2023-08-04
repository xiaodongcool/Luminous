using Serilog.Events;

namespace LangM.AspNetCore
{
    /// <summary>
    ///     查询字符串 (属性名:es_QueryString)
    /// </summary>
    public class QueryStringEnricher : CustomerLogEventEnricher
    {
        /// <summary>
        ///     属性名称
        /// </summary>
        protected override string PropertyName => "QueryString";

        /// <summary>
        ///     获取属性值
        /// </summary>
        protected override string GetPropertyValue(LogEvent logEvent, HttpContext httpContext) => httpContext?.Request?.QueryString.ToString();
    }
}