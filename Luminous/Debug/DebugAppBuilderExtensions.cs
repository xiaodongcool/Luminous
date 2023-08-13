using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NPOI.SS.Formula.Functions;
using System.Text;

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
                    var internalConfiguration = Global.GetConfig("Luminous:_source_");

                    var result = new Result<object>(ResultStatus.Success, new { redis = redisStatus, apollo, appsettings, preferential, internalConfiguration }, "");
                    var resultJson = JsonConvert.SerializeObject(result, Global.JsonSerializerSettings);

                    logger.LogInformation($"luminous-debug reponse：{resultJson}");

                    x.Response.ContentType = "text/plain; charset=utf-8";

                    await x.Response.WriteAsync(resultJson);
                });

                app.MapGet("luminous-debug-cfg", async (x) =>
                {
                    var service = x.RequestServices;
                    var configuration = service.GetRequiredService<IConfiguration>();
                    var source = x.Request.Query["source"].ToString().ToLower();

                    var result = new StringBuilder();

                    if (source == "global")
                    {
                        configuration = Global.Configuration;
                    }

                    var index = 1;

                    PrintConfiguration(configuration, ref index, result);
                    x.Response.ContentType = "text/plain; charset=utf-8";

                    await x.Response.WriteAsync(result.ToString());
                });

                app.MapGet("luminous-debug-cfg-query", async (x) =>
                {
                    var service = x.RequestServices;
                    var configuration = service.GetRequiredService<IConfiguration>();

                    var key = x.Request.Query["key"].ToString();
                    var source = x.Request.Query["source"].ToString().ToLower();
                    var type = x.Request.Query["type"].ToString();

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

                    x.Response.ContentType = "text/plain; charset=utf-8";
                    await x.Response.WriteAsync(result ?? "值为 NULL", Encoding.UTF8);
                });
            }

            return app;
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

    public static class A
    {
        public static IEndpointConventionBuilder MapGet<T>(this IEndpointRouteBuilder endpoints, string pattern, Func<IServiceProvider, IResult<T>> getResultFunc)
        {
            return endpoints.MapGet(pattern, async (httpContext) =>
            {
                var result = getResultFunc(httpContext.RequestServices);
                var resultJson = JsonConvert.SerializeObject(result, Global.JsonSerializerSettings);
                httpContext.Response.ContentType = "text/plain; charset=utf-8";
                await x.Response.WriteAsync(resultJson);
            });
        }
    }
}