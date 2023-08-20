using Luminous;
using Serilog;

namespace ServiceB.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                WebApplication.CreateBuilder(args)
                    .AddLuminous()
                    .Build()
                    .UseLuminous()
                    .Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "应用程序启动失败");
            }
        }
    }
}