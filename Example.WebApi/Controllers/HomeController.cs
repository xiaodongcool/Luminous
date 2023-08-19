using Luminous;
using Luminous.Controller;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Example.WebApi.Controllers
{
    public class HomeController : ApiController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRedis _redis;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IRedis redis, IConfiguration configuration)
        {
            _logger = logger;
            _redis = redis;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<object> Index()
        {
            var redis = false;

            try
            {
                await _redis.SetAsync("test", "hello world!", 10);
                redis = await _redis.GetAsync("test") == "hello world!";
            }
            catch (Exception e)
            {
                _logger.LogError(e, "redis 启动连接失败");
            }

            var apollo = _configuration.GetValue<bool>("Luminous:_apollo_");
            var appsettings = _configuration.GetValue<bool>("Luminous:_appsettings_");
            var preferential = _configuration.GetValue<string>("Luminous:_source_");
            var internalConfiguration = Global.GetConfig("Luminous:_source_");

            var result = new { redis, apollo, appsettings, preferential, internalConfiguration };

            _logger.LogInformation($"Home/Index：{JsonConvert.SerializeObject(result)}");

            return result;
        }

        [HttpGet]
        public async Task<int> Customer()
        {
            var a = HttpContext.RequestServices.GetRequiredService<ICustomerMyInterface1>().f(1);
            var b = HttpContext.RequestServices.GetRequiredService<ICustomerMyInterface2>().f(2);

            return a + b;
        }
        [HttpGet("Customer3")]
        public async Task<int> Customer4()
        {
            var a = HttpContext.RequestServices.GetRequiredService<ICustomerMyInterface1>().f(1);
            var b = HttpContext.RequestServices.GetRequiredService<ICustomerMyInterface2>().f(2);

            return a + b;
        }
    }
    public class UserResponse
    {
        public UserResponse(string name, Gender gender, Role role)
        {
            Name = name;
            CreateTime = DateTime.Now;
            Gender = gender;
            Role = new RoleResponse(role);
        }

        public UserResponse(string name, Gender gender)
        {
            Name = name;
            CreateTime = DateTime.Now;
            Gender = gender;
        }

        public UserResponse(string name, Role role)
        {
            Name = name;
            CreateTime = DateTime.Now;
            Role = new RoleResponse(role);
        }


        public UserResponse(string name)
        {
            Name = name;
            CreateTime = DateTime.Now;
        }

        public string Name { get; set; } = null!;
        public DateTime CreateTime { get; set; }
        public Gender Gender { get; set; }
        public RoleResponse Role { get; set; }
    }

    public class RoleResponse
    {
        public RoleResponse(Role role)
        {
            Role = role;
        }

        public Role Role { get; set; }
    }

    public enum Gender
    {
        [Meaning("男")]
        Male = 1,
        [Meaning("女")]
        Female = 2,
    }

    public enum Role
    {
        [Meaning("用户")]
        User = 1,
        [Meaning("管理员")]
        Admin = 2
    }

}