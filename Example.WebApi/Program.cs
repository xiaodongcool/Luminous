using AspectCore.DynamicProxy;
using AspectCore.Extensions.DependencyInjection;
using Luminous;
using Luminous.Configuration;
using Luminous.DynamicProxy;
using Luminous.Exception;
using Luminous.Logging;

namespace Example.WebApi
{
    public interface IMyInterface1 { }
    public interface IMyInterface2 { }

    public interface ICustomerMyInterface1 : IMyInterface1
    {
        int f(int value);
    }

    public interface ICustomerMyInterface2 : IMyInterface2
    {
        int f(int value);
    }

    public class CustomerLuminousInterceptor1 : LuminousInterceptor
    {
        public CustomerLuminousInterceptor1()
        {

        }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            context.ReturnValue = (int)context.Parameters[0];
        }
    }

    public class CustomerLuminousInterceptor2 : LuminousInterceptor
    {
        public CustomerLuminousInterceptor2()
        {

        }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            context.ReturnValue = ((int)context.Parameters[0]) * 2;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddLuminousDynamicProxyInterface<IMyInterface1, CustomerLuminousInterceptor1>();
            builder.AddLuminousDynamicProxyInterface<IMyInterface2, CustomerLuminousInterceptor2>();

            builder.Services.ConfigureDynamicProxy();

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