using Serilog.Events;

namespace LangM.AspNetCore
{
    /// <summary>
    ///      用户名
    /// </summary>
    public class UserNameEnricher : CustomerLogEventEnricher
    {
        /// <summary>
        ///     属性名称
        /// </summary>
        protected override string PropertyName => "UserName";

        /// <summary>
        ///     获取属性值
        /// </summary>
        protected override string GetPropertyValue(LogEvent logEvent, HttpContext httpContext) => httpContext?.User?.FindFirst(TokenClaimTypeNames.UserName)?.Value;
    }
}