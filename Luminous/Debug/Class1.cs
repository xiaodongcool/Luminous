using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Builder
{
    public static class DebugAppBuilderExtensions
    {
        public static IApplicationBuilder UseLuminousDebug(this WebApplication app)
        {
            if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("Luminous:EnableDebug"))
            {
                app.MapGet("luminous-debug", async (x) =>
                {
                    var service = x.RequestServices;
                    var logger = service.GetRequiredService<ILogger<object>>();
                    var redis = service.GetRequiredService<IRedis>();
                    var configuration = service.GetRequiredService<IConfiguration>();

                    var redisStatus = false;

                    try
                    {
                        await redis.SetAsync("test", "hello world!", 10);
                        redisStatus = await redis.GetAsync("test") == "hello world!";
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "redis 启动连接失败");
                    }

                    var apollo = configuration.GetValue<bool>("Luminous:_apollo_");
                    var appsettings = configuration.GetValue<bool>("Luminous:_appsettings_");
                    var preferential = configuration.GetValue<string>("Luminous:_source_");
                    var internalConfiguration = CONFIGURATION.Get("Luminous:_source_");

                    var result = new Result<object>(ResultStatus.Success, new { redis = redisStatus, apollo, appsettings, preferential, internalConfiguration }, "");
                    var resultJson = JsonConvert.SerializeObject(result, Global.JsonSerializerSettings);

                    logger.LogInformation($"luminous-debug reponse：{resultJson}");

                    await x.Response.WriteAsync(resultJson);
                });
            }

            return app;
        }
    }
}