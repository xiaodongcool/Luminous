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
            //  雪花id
            services.AddLuminousUniqueId();
            //  token
            //services.AddJwtBearToken();
            //  redis 缓存
            services.AddLuminousRedis();
            //  mvc
            services.AddControllers();
            //  响应报文契约
            services.AddLuminousResult();
            //  模型验证
            services.AddModelValidation();
            //  请求报文响应报文 json 序列化规范
            services.AddLuminousJsonFormat();
            //  注册所有服务和仓储
            //services.AddApplication();
            //  数据库连接字符串
            //services.AddConnectionString<ConnectionStringProvider>();
            //  添加数据库连接工厂
            //services.AddSqlSugarUnitOfWork();
            //services.AddDapperUnitOfWork();
            //  赋值器
            //services.AddAssignment();
            //  AutoMapper
            services.AddLuminousAutoMapper();

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