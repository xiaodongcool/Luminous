using AutoMapper.Configuration.Conventions;
using Luminous;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.ComponentModel;
using System.IO;

namespace Example.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HomeController : ApiController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<UserResponse> Get1()
        {
            _logger.LogInformation("hello world");
            return new UserResponse("����", Gender.Male, Role.Admin);
        }
    }

    [ApiController]
    [Route("[controller]/[action]")]
    public class JsonController : ApiController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserResponse[] ServiceInfos = new[]
        {
            new UserResponse("����", Gender.Male, Role.Admin),
            new UserResponse("����", Gender.Female, Role.User),
            new UserResponse("����"),
            new UserResponse("Jack"),
            new UserResponse("Wendy", Gender.Male),
            new UserResponse("Merry", Gender.Female),
            new UserResponse("Doris", Role.Admin),
            new UserResponse("Iven", Role.User),
        };

        public JsonController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
            throw new FailException("����״̬�쳣");
        }


        [HttpGet]
        public async Task<string> Get15()
        {
            throw new FailException("����״̬�쳣");
        }

        [HttpGet]
        public async Task<string> Get16()
        {
            return "hello world!";
        }

        [HttpGet]
        public async Task<string> Get17()
        {
            SetResultStatusAndMessage(ResultStatus.Fail, "������һЩ����");
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
            SetResultStatusAndMessage(ResultStatus.Fail, "������һЩ����");
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
                Name = "����",
                Gender = Gender.Male,
            };
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
        [Meaning("��")]
        Male = 1,
        [Meaning("Ů")]
        Female = 2,
    }

    public enum Role
    {
        [Meaning("�û�")]
        User = 1,
        [Meaning("����Ա")]
        Admin = 2
    }

}