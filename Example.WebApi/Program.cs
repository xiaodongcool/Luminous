using Luminous;

namespace Example.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddLuminous();

            builder.AddLuminousLogging();
            builder.AddLuminousConfiguration();

            ConfigureServices(builder.Services);
            Configure(builder.Build());
        }

        static void ConfigureServices(IServiceCollection services)
        {
            services.AddLuminousUniqueId();
            services.AddLuminousRedis();
            services.AddControllers();
            services.AddLuminousResult();
            services.AddLuminousModelValidation();
            services.AddLuminousJsonFormat();
            services.AddLuminousAutoMapper();

            //  token
            //services.AddJwtBearToken();
            //  注册所有服务和仓储
            //services.AddApplication();
            //  数据库连接字符串
            //services.AddConnectionString<ConnectionStringProvider>();
            //  添加数据库连接工厂
            //services.AddSqlSugarUnitOfWork();
            //services.AddDapperUnitOfWork();

        }

        static void Configure(WebApplication app)
        {
            app.UseLuminousServiceLocator();

            app.UseCatchGlobalException();
            //  身份认证
            app.UseAuthorization();
            //  mvc
            app.MapControllers();

            app.UseLuminousDebug();

            //  启动
            try
            {
                app.Run();
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}