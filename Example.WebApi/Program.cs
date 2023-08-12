using Luminous;

namespace Example.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddLogging();
            builder.AddConfiguration();

            ConfigureServices(builder.Services);
            Configure(builder.Build());
        }

        static void ConfigureServices(IServiceCollection services)
        {
            //  ��ӱر���һЩ���÷���
            services.AddRequired();
            //  ѩ��id
            services.AddWorkId();
            //  token
            //services.AddJwtBearToken();
            //  redis ����
            services.AddFullRedis();
            //  mvc
            services.AddControllers();
            //  ��Ӧ������Լ
            services.AddContactFilter();
            //  ģ����֤
            services.AddModelValidation();
            //  ��������Ӧ���� json ���л��淶
            services.AddJsonSerializer();
            //  HttpContext
            services.AddHttpContextSuperman();
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
            services.RegisterAutoMapper();

        }

        static void Configure(WebApplication app)
        {
            app.UseCatchGlobalException();
            //  �����֤
            app.UseAuthorization();
            //  mvc
            app.MapControllers();

            //  ����
            app.Run();
        }
    }
}