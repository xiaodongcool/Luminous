using Microsoft.AspNetCore.Builder;

namespace Luminous
{
    public static class LuminousServiceCollectionExtensions
    {
        public static WebApplicationBuilder AddLuminousMicroservice(this WebApplicationBuilder builder)
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

            builder.AddLuminous();

            builder.Services.AddLuminousNacos();

            return builder;
        }
    }
}