using Luminous;
using Luminous.Controller;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace Example.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ResultController : ApiController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserResponse[] ServiceInfos = new[]
        {
            new UserResponse("张三", Gender.Male, Role.Admin),
            new UserResponse("李四", Gender.Female, Role.User),
            new UserResponse("王五"),
            new UserResponse("Jack"),
            new UserResponse("Wendy", Gender.Male),
            new UserResponse("Merry", Gender.Female),
            new UserResponse("Doris", Role.Admin),
            new UserResponse("Iven", Role.User),
        };

        public ResultController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<UserResponse?> Get0()
        {
            return null;
        }

        [HttpGet]
        public async Task<UserResponse> Get1()
        {
            return ServiceInfos[0];
        }

        [HttpGet]
        public async Task<UserResponse[]> Get2()
        {
            return ServiceInfos;
        }

        [HttpGet]
        public async Task<IList<UserResponse>> Get3()
        {
            return ServiceInfos.ToList();
        }

        [HttpGet]
        public async Task<List<UserResponse>> Get4()
        {
            return ServiceInfos.ToList();
        }

        [HttpGet]
        public async Task<HashSet<UserResponse>> Get5()
        {
            return ServiceInfos.ToHashSet();
        }

        [HttpGet]
        public async Task<IEnumerable<UserResponse>> Get6()
        {
            return ServiceInfos;
        }

        [HttpGet]
        public async Task<IEnumerable<object>> Get61()
        {
            return ServiceInfos;
        }

        [HttpGet]
        public async Task<object> Get62()
        {
            return ServiceInfos;
        }

        [HttpGet]
        public async Task<IEnumerable> Get7()
        {
            return ServiceInfos;
        }

        [HttpGet]
        public async Task<Page<UserResponse>> Get8()
        {
            return new Page<UserResponse>(10, ServiceInfos);
        }

        [HttpGet]
        public async Task<int> Get9()
        {
            return 1;
        }

        [HttpGet]
        public async Task<Page<int>> Get10()
        {
            return new Page<int>(10, new[] { 1, 2, 3 });
        }

        [HttpGet]
        public async Task<Page<int>> Get11()
        {
            return null;
        }

        [HttpGet]
        public async Task Get12()
        {
        }

        [HttpGet]
        public async Task Get13()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task Get14()
        {
            throw new FailException("订单状态异常");
        }


        [HttpGet]
        public async Task<string> Get15()
        {
            throw new FailException("订单状态异常");
        }

        [HttpGet]
        public async Task<string> Get16()
        {
            return "hello world!";
        }

        [HttpGet]
        public async Task<string> Get17()
        {
            SetResultStatusAndMessage(ResultStatus.Fail, "发生了一些错误");
            return "hello world!";
        }

        [HttpGet]
        public void Get18()
        {

        }

        [HttpGet]
        public void Get19()
        {
            throw new FailException("12");
        }

        [HttpGet]
        public void Get20()
        {
            SetResultStatusAndMessage(ResultStatus.Fail, "发生了一些错误");
        }

        [HttpGet]
        public IActionResult Get21()
        {
            var filepath = @"D:\1.txt";

            var file = new FileStream(filepath, FileMode.Open);
            return File(file, "application/octet-stream", "1.txt");
        }

        [HttpGet]
        public object Get22()
        {
            return new
            {
                Name = "张三",
                Gender = Gender.Male,
            };
        }
    }

}