using Serilog.Events;

namespace Luminous
{
    /// <summary>
    ///      账户唯一标识符 (属性名:es_AccountUniqueId)
    /// </summary>
    public class AccountUniqueIdEnricher : CustomerLogEventEnricher
    {
        /// <summary>
        ///     属性名称
        /// </summary>
        protected override string PropertyName => "AccountUniqueId";

        /// <summary>
        ///     获取属性值
        /// </summary>
        protected override string GetPropertyValue(LogEvent logEvent, HttpContext httpContext) => httpContext?.User?.FindFirst(TokenClaimTypeNames.AccountUniqueId)?.Value;
    }
}