using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AspNetCoreSerializerServiceExtensions
    {
        /// <summary>
        ///     设置接口响应 json 的序列化配置
        /// </summary>
        public static void AddLuminousJsonFormat(this IServiceCollection services, Action<SerializerOptions>? configure = null)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    var serializerOptions = new SerializerOptions();

                    configure?.Invoke(serializerOptions);

                    if (serializerOptions.Named == SerializerNamed.Hump)
                    {
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    }

                    if (serializerOptions.Named == SerializerNamed.SnakeCase)
                    {
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new SnakeCaseNamingStrategy()
                        };
                    }

                    if (!string.IsNullOrEmpty(serializerOptions.DateTimeFormat))
                    {
                        options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = serializerOptions.DateTimeFormat });
                    }

                    if (serializerOptions.IgnoreNullProperty)
                    {
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    }

                    Global.JsonSerializerSettings = options.SerializerSettings;
                });
        }
    }
}
