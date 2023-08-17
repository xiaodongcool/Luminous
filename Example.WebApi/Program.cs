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