using Luminous;
using Microsoft.AspNetCore.Mvc;

namespace Example.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Json2Controller : ApiController
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

        public Json2Controller(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IResult<UserResponse>> Get1()
        {
            return new Result<UserResponse>(ResultStatus.Success, ServiceInfos[0], null);
        }

        [HttpGet]
        public async Task<IResult<UserResponse[]>> Get2()
        {
            return new Result<UserResponse[]>(ResultStatus.Success, ServiceInfos, null);
        }

        [HttpGet]
        public async Task<IResult<IList<UserResponse>>> Get3()
        {
            return new Result<IList<UserResponse>>(ResultStatus.Success, ServiceInfos.ToList(), null);
        }


        [HttpGet]
        public async Task<IResult<List<UserResponse>>> Get4()
        {
            return new Result<List<UserResponse>>(ResultStatus.Success, ServiceInfos.ToList(), null);
        }

        [HttpGet]
        public async Task<IResult<HashSet<UserResponse>>> Get5()
        {
            return new Result<HashSet<UserResponse>>(ResultStatus.Success, ServiceInfos.ToHashSet(), null);
        }

        [HttpGet]
        public async Task<IResult<IEnumerable<UserResponse>>> Get6()
        {
            //  没有翻译枚举
            return new Result<IEnumerable<UserResponse>>(ResultStatus.Success, ServiceInfos, null);
        }

        [HttpGet]
        public async Task<IResult<Page<UserResponse>>> Get8()
        {
            return new Result<Page<UserResponse>>(ResultStatus.Success, new Page<UserResponse>(10, ServiceInfos), null);
        }

        [HttpGet]
        public async Task<IResult<int>> Get9()
        {
            return new Result<int>(ResultStatus.Success, 1, null);
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
        public async Task<IResult<object>> Get12()
        {
            return new Result<object>(ResultStatus.Success, null, null);
        }

        [HttpGet]
        public async Task<IResult<object>> Get13()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IResult<object>> Get14()
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