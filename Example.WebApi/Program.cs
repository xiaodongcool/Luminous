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
            //  ��ӱر���һЩ���÷���
            services.AddRequired();
            //  ѩ��id
            services.AddLuminousUniqueId();
            //  token
            //services.AddJwtBearToken();
            //  redis ����
            services.AddLuminousRedis();
            //  mvc
            services.AddControllers();
            //  ��Ӧ������Լ
            services.AddLuminousResult();
            //  ģ����֤
            services.AddModelValidation();
            //  ��������Ӧ���� json ���л��淶
            services.AddLuminousJsonFormat();
            //  HttpContext
            services.AddLuminousHttpContexter();
            //  ע�����з���Ͳִ�
            //services.AddApplication();
            //  ���ݿ������ַ���
            //services.AddConnectionString<ConnectionStringProvider>();
            //  ������ݿ����ӹ���
            //services.AddSqlSugarUnitOfWork();
            //services.AddDapperUnitOfWork();
            //  ��ֵ��
            //services.AddAssignment();
            //  AutoMapper
            services.AddLuminousAutoMapper();


        }

        static void Configure(WebApplication app)
        {
            app.UseCatchGlobalException();
            //  �����֤
            app.UseAuthorization();
            //  mvc
            app.MapControllers();

            app.UseLuminousDebug();

            //  ����
            app.Run();
        }
    }
}