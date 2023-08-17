using Luminous.Logging.Options;
using Microsoft.AspNetCore.Hosting;

namespace Luminous.Configuration
{
    /// <summary>
    ///     系统配置列表
    /// </summary>
    internal static class CONFIGS
    {
        internal static LoggingOptions? Log => Global.GetConfig<LoggingOptions>("Luminous:Log");
        internal static JwtBearOptions? Jwtbear => Global.GetConfig<JwtBearOptions>("jwtBear");
        internal static RedisOptions? Redis => Global.GetConfig<RedisOptions>("Luminous:Redis");
    }
}
