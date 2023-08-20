using Microsoft.Extensions.DependencyInjection;

namespace Luminous
{
    public static class LuminousServiceCollectionExtensions
    {
        public static WebApplicationBuilder AddLuminous(this WebApplicationBuilder builder)
        {
            //  token
            //services.AddJwtBearToken();
            //  注册所有服务和仓储
            //services.AddApplication();
            //  数据库连接字符串
            //services.AddConnectionString<ConnectionStringProvider>();
            //  添加数据库连接工厂
            //services.AddSqlSugarUnitOfWork();
            //services.AddDapperUnitOfWork();

            builder.AddLuminousLogging();
            builder.AddLuminousAssemblyMetadata();
            builder.AddLuminousConfiguration();

            builder.Services.AddRelection();
            builder.Services.AddLuminousHttpContexter();
            builder.Services.AddLuminousUniqueId();
            builder.Services.AddLuminousRedis();
            builder.Services.AddLuminousResult();
            builder.Services.AddLuminousModelValidation();
            builder.Services.AddLuminousJsonFormat();
            builder.Services.AddLuminousAutoMapper();
            builder.Services.AddControllers();
            builder.Services.AddLuminousHttpInvocation();

            return builder;
        }
    }
}
