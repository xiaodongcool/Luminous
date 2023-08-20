using Luminous;
using Serilog;

namespace ServiceC.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                WebApplication.CreateBuilder(args)
                    .AddLuminousMicroservice()
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