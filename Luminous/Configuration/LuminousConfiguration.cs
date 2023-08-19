namespace Luminous
{
    public partial class LuminousConfiguration
    {
        public static LoggingOptions? Log => Global.GetConfig<LoggingOptions>("Luminous:Log");
        public static JwtBearOptions? Jwtbear => Global.GetConfig<JwtBearOptions>("jwtBear");
        public static RedisOptions? Redis => Global.GetConfig<RedisOptions>("Luminous:Redis");
    }
}
