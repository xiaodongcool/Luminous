using AspectCore.DynamicProxy;
using AspectCore.Extensions.DependencyInjection;
using Luminous;
using Luminous.DynamicProxy;

namespace Example.WebApi
{
    public interface IMyInterface { }

    public interface ICustomerMyInterface
    {
        int f();
    }

    public class CustomerLuminousInterceptor : LuminousInterceptor
    {
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddLuminousDynamicProxy<IMyInterface, CustomerLuminousInterceptor>(ServiceLifetime.Transient);

            builder.AddLuminousLogging();
            builder.AddLuminousConfiguration();

            ConfigureServices(builder.Services);
            Configure(builder.Build());
        }

        static void ConfigureServices(IServiceCollection services)
        {
            //  添加必备的一些内置服务
            services.AddRequired();
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
            //  HttpContext
            services.AddLuminousHttpContexter();
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
            app.UseCatchGlobalException();
            //  身份认证
            app.UseAuthorization();
            //  mvc
            app.MapControllers();

            app.UseLuminousDebug();

            //  启动
            app.Run();
        }
    }
}