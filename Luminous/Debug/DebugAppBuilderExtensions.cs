using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
    public static class DebugAppBuilderExtensions
    {
        public static IApplicationBuilder UseLuminousDebug(this WebApplication app)
        {
            if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("Luminous:EnableDebug"))
            {
                app.MapGetResult("luminous-debug", async (request, service) => await GetSystemStatus(service));
                app.MapGetResult("luminous-debug-cfg", async (request, service) => await GetConfig(request, service));
                app.MapGetResult("luminous-debug-cfg-query", async (request, service) => await GetConfigQuery(request, service));
            }

            return app;
        }

        public static Task<string> GetConfigQuery(HttpRequest request, IServiceProvider service)
        {
            var configuration = service.GetRequiredService<IConfiguration>();

            var key = request.Query["key"].ToString();
            var source = request.Query["source"].ToString().ToLower();
            var type = request.Query["type"].ToString();

            if (source == "global")
            {
                configuration = Global.Configuration;
            }

            string? result;

            if (string.IsNullOrEmpty(key))
            {
                result = "key 不能为空";
            }
            else
            {
                var section = configuration.GetSection(key);
                var objectType = TypeContainer.FindAll().FirstOrDefault(x => x.FullName == type);

                if (section.Value == null)
                {
                    if (objectType == null)
                    {
                        result = $"key '{key}' 不存在";
                    }
                    else
                    {
                        var objectValue = section.Get(objectType, x => { });
                        result = JsonConvert.SerializeObject(objectValue);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(type))
                    {
                        result = section.Value;
                    }
                    else
                    {

                        if (objectType == null)
                        {
                            result = $"type '{type}' 不存在,请使用 FullName";
                        }
                        else
                        {
                            var objectValue = JsonConvert.DeserializeObject(section.Value, objectType);

                            if (objectValue == null)
                            {
                                objectValue = section.Get(objectType);
                            }

                            result = JsonConvert.SerializeObject(objectValue);
                        }
                    }
                }
            }

            return Task.FromResult(result);
        }

        private static Task<string> GetConfig(HttpRequest request, IServiceProvider service)
        {
            var configuration = service.GetRequiredService<IConfiguration>();
            var source = request.Query["source"].ToString().ToLower();

            var result = new StringBuilder();

            if (source == "global")
            {
                configuration = Global.Configuration;
            }

            var index = 1;

            PrintConfiguration(configuration, ref index, result);

            return Task.FromResult(result.ToString());
        }

        private static async Task<string> GetSystemStatus(IServiceProvider service)
        {
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
            var internalConfiguration = Global.GetConfig("Luminous:_source_");

            var result = new { redis = redisStatus, apollo, appsettings, preferential, internalConfiguration };

            return JsonConvert.SerializeObject(result, Global.JsonSerializerSettings);
        }

        private static void PrintConfiguration(IConfiguration configuration, ref int index, StringBuilder result, string parentKey = "")
        {
            foreach (var section in configuration.GetChildren())
            {
                var key = string.IsNullOrEmpty(parentKey) ? section.Key : $"{parentKey}:{section.Key}";
                var value = section.Value;

                result.AppendLine($"{index++} {key} = {value}");

                PrintConfiguration(section, ref index, result, key);
            }
        }
    }
}