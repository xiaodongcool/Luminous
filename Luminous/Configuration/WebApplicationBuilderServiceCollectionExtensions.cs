﻿using Com.Ctrip.Framework.Apollo;

namespace Luminous.Configuration
{
    public static class WebApplicationBuilderServiceCollectionExtensions
    {
        /// <summary>
        ///     添加阿波罗配置中心
        /// </summary>
        public static IConfiguration AddLuminousConfiguration(this WebApplicationBuilder builder)
        {
            try
            {
                builder.Configuration
                    .AddApollo((builder.Configuration as IConfigurationBuilder).Build().GetSection("Luminous:Apollo"))
                    .AddDefault();
            }
            catch (Exception e)
            {
                Serilog.Log.Warning(e, "Apollo 阿波罗配置中心初始化失败，配置信息从 appsettings.json 中读取");
            }

            var configuration = (builder.Configuration as IConfigurationBuilder).Build();
            return configuration;
        }
    }
}