using Luminous;
using Microsoft.AspNetCore.Mvc;

namespace Example.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ApiController
    {
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

    [ApiController]
    [Route("[controller]/[action]")]
    public class JsonController : ApiController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContactProvider _contactProvider;

        public JsonController(ILogger<HomeController> logger, IContactProvider contactProvider)
        {
            _logger = logger;
            _contactProvider = contactProvider;
        }

        [HttpGet]
        public async Task<ServiceInfo> Get1()
        {
            return new ServiceInfo
            {
                Name = "Example.WebApi",
                DateTime = DateTime.Now,
                DayOfWeek = DateTime.Now.DayOfWeek,
                Greetings = "ÄãºÃ"
            };
        }

        [HttpGet]
        public async Task<IContact<ServiceInfo>> Get2()
        {
            return _contactProvider.Create(WebApiStatusCode.Success, new ServiceInfo
            {
                Name = "Example.WebApi",
                DateTime = DateTime.Now,
                DayOfWeek = DateTime.Now.DayOfWeek,
                Greetings = "ÄãºÃ"
            });
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