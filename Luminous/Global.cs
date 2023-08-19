using Com.Ctrip.Framework.Apollo;

namespace Luminous
{
    public static class Global
    {
        private static IConfiguration configuration;

        static Global()
        {
            try
            {
                var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                configurationBuilder.AddApollo(configurationBuilder.Build().GetSection("Luminous:Apollo")).AddDefault();
                configuration = configurationBuilder.Build();
            }
            catch
            {
                configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            }
        }

        public static JsonSerializerSettings? JsonSerializerSettings { get; set; }

        public static IConfiguration Configuration => configuration;

        /// <summary>
        ///     读取配置
        /// </summary>
        public static T? GetConfig<T>(string key)
        {
            ArgumentGuard.CheckForNull(key, nameof(key));

            var value = configuration[key];

            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            else
            {
                return configuration.GetSection(key).Get<T>();
            }
        }

        /// <summary>
        ///     读取配置
        /// </summary>
        public static string? GetConfig(string key)
        {
            return configuration[key];
        }

        public static ISolutionAssemblyMetadata? Solution { get; internal set; }
    }
}
