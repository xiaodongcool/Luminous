using Luminous;
using Microsoft.AspNetCore.Mvc;

namespace Example.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ApiController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ServiceInfo> Get()
        {
            return new ServiceInfo
            {
                Name = "Example.WebApi",
                DateTime = DateTime.Now,
                DayOfWeek = DateTime.Now.DayOfWeek,
                Greetings = "ÄãºÃ"
            };
        }
    }

    public class ServiceInfo
    {
        public string Name { get; set; } = null!;
        public DateTime DateTime { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public string? Greetings { get; set; }
    }
}