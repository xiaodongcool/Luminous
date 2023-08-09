using Microsoft.AspNetCore.Hosting;

namespace Luminous
{
    public class Application : IApplication
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Application(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public Env Env
        {
            get
            {
                return Env.Development;
            }
        }
    }
}
