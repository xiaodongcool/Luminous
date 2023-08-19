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
            app.UseLuminousServiceLocator();

            app.UseCatchGlobalException();
            //  �����֤
            app.UseAuthorization();
            //  mvc
            app.MapControllers();

            app.UseLuminousDebug();

            //  ����
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