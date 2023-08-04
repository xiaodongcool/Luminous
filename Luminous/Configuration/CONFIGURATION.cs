namespace Luminous
{
    /// <summary>
    ///     内部配置接口
    /// </summary>
    internal static class CONFIGURATION
    {
        private static IConfiguration configurationCenter;
        private static IConfiguration appsettings;

        /// <summary>
        ///     appsettings.json
        /// </summary>
        internal static IConfiguration AppSettings
        {
            get
            {
                if (appsettings == null)
                {
                    appsettings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                }
                return appsettings;
            }
        }

        /// <summary>
        ///     配置中心
        /// </summary>
        internal static IConfiguration ConfigurationCenter
        {
            get
            {
                if (configurationCenter == null)
                {
                    throw new Exception("配置中心未初始化");
                }
                return configurationCenter;
            }
            set
            {
                configurationCenter = value;
            }
        }

        /// <summary>
        ///     读取配置
        /// </summary>
        internal static T Get<T>(string key)
        {
            ArgumentChecker.ThrowIfNull(key, nameof(key));

            if (configurationCenter != null)
            {
                var value = configurationCenter[key];

                if (!string.IsNullOrEmpty(value))
                {
                    //  可能报错 读的肯定是 configuration center
                    return JsonConvert.DeserializeObject<T>(value);
                }
                else
                {
                    //  可能是 null 也可能报错 但读的肯定是 appsettings.json
                    return configurationCenter.GetSection(key).Get<T>();
                }
            }
            else
            {
                return AppSettings.GetSection(key).Get<T>();
            }
        }

        /// <summary>
        ///     读取配置
        /// </summary>
        internal static string Get(string key)
        {
            return (configurationCenter ?? AppSettings)[key];
        }
    }
}
