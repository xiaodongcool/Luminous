using Microsoft.AspNetCore.Hosting;

namespace LangM.AspNetCore
{
    /// <summary>
    ///     系统配置列表
    /// </summary>
    internal static class CONFIGS
    {
        internal static LoggingOptions Log => CONFIGURATION.Get<LoggingOptions>("log");
        internal static JwtBearOptions Jwtbear => CONFIGURATION.Get<JwtBearOptions>("jwtBear");
        internal static RedisOptions Redis => CONFIGURATION.Get<RedisOptions>("redis");
    }
}
