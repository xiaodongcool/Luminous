using Serilog.Events;

namespace LangM.AspNetCore
{
    /// <summary>
    ///     客户端代理信息 (属性名:es_ClientAgent)
    /// </summary>
    public class ClientAgentEnricher : CustomerLogEventEnricher
    {
        /// <summary>
        ///     属性名称
        /// </summary>
        protected override string PropertyName => "ClientAgent";

        /// <summary>
        ///     获取属性值
        /// </summary>
        protected override string GetPropertyValue(LogEvent logEvent, HttpContext httpContext) => httpContext?.Request?.Headers?["User-Agent"].ToString();
    }
}
