using Microsoft.Extensions.Logging;

namespace Luminous
{
    public class LuminousUtil
    {
        public static async Task<object> GetStatus()
        {
            var redisService = ServiceLocator.GetScopedService<IRedis>();
            var configuration = ServiceLocator.GetScopedService<IConfiguration>();
            var logger = ServiceLocator.GetScopedService<ILogger<LuminousUtil>>();

            var redis = false;

            try
            {
                await redisService.SetAsync("test", "hello world!", 10);
                redis = await redisService.GetAsync("test") == "hello world!";
            }
            catch (Exception e)
            {
                logger.LogError(e, "redis 启动连接失败");
            }

            var apollo = configuration.GetValue<bool>("Luminous:_apollo_");
            var appsettings = configuration.GetValue<bool>("Luminous:_appsettings_");
            var preferential = configuration.GetValue<string>("Luminous:_source_");
            var internalConfiguration = Global.GetConfig("Luminous:_source_");

            var result = new { redis, apollo, appsettings, preferential, internalConfiguration };

            logger.LogInformation($"Home/Index：{JsonConvert.SerializeObject(result)}");

            return result;
        }
    }
}
