namespace Luminous
{
    /// <summary>
    ///     系统配置列表
    /// </summary>
    public static class LuminousConfiguration
    {
        public static LoggingOptions? Log => Global.GetConfig<LoggingOptions>("Luminous:Log");
        public static JwtBearOptions? Jwtbear => Global.GetConfig<JwtBearOptions>("jwtBear");
        public static RedisOptions? Redis => Global.GetConfig<RedisOptions>("Luminous:Redis");
    }
}
