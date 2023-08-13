//using Com.Ctrip.Framework.Apollo;

//namespace Luminous
//{
//    /// <summary>
//    ///     内部配置接口
//    /// </summary>
//    public static class CONFIGURATION
//    {
//        private static IConfiguration configuration;

//        static CONFIGURATION()
//        {
//            try
//            {
//                var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
//                configurationBuilder.AddApollo(configurationBuilder.Build().GetSection("Luminous:Apollo")).AddDefault();
//                configuration = configurationBuilder.Build();
//            }
//            catch
//            {
//                configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
//            }
//        }

//        /// <summary>
//        ///     读取配置
//        /// </summary>
//        public static T Get<T>(string key)
//        {
//            ArgumentChecker.ThrowIfNull(key, nameof(key));

//            var value = configuration[key];

//            if (!string.IsNullOrEmpty(value))
//            {
//                //  可能报错 读的肯定是 configuration center
//                return JsonConvert.DeserializeObject<T>(value);
//            }
//            else
//            {
//                //  可能是 null 也可能报错 但读的肯定是 appsettings.json
//                return configuration.GetSection(key).Get<T>();
//            }
//        }

//        /// <summary>
//        ///     读取配置
//        /// </summary>
//        public static string Get(string key)
//        {
//            return (configuration ?? configuration)[key];
//        }
//    }
//}
