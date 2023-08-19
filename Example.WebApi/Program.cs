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
            //  ע�����з���Ͳִ�
            //services.AddApplication();
            //  ���ݿ������ַ���
            //services.AddConnectionString<ConnectionStringProvider>();
            //  ������ݿ����ӹ���
            //services.AddSqlSugarUnitOfWork();
            //services.AddDapperUnitOfWork();

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