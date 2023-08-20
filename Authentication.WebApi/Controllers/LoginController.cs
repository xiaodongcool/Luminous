using Authentication.WebApi.Model;
using AutoMapper;
using Luminous;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.WebApi.Controllers
{
    public class AuthenticationController : LuminousController
    {
        private readonly IMapper mapper;

        public AuthenticationController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<LoginResponse?> Login([FromBody] LoginRequest request)
        {
            var user = await GetUserByAccount(request.UserAccount);

            if (user == null)
            {
                throw new FailException("账号不存在");
            }
            else if (user.Password != request.Password)
            {
                SetResultStatusAndMessage(ResultStatus.Fail, "密码错误");
                return null;
            }
            else
            {
                return mapper.Map<LoginResponse>(request);
            }

        }

        private Task<User?> GetUserByAccount(string userAccount)
        {
            var users = new List<User>()
            {
                new User(1,"jack","abc"),
                new User(1,"emma","abc"),
            };

            return Task.FromResult(users.FirstOrDefault(x => x.UserAccount == userAccount));
        }
    }

    public class User
    {
        public User()
        {
        }

        public User(long id, string userAccount, string password)
        {
            Id = id;
            UserAccount = userAccount;
            Password = password;
        }

        public long Id { get; set; }
        public string UserAccount { get; set; }
        public string Password { get; set; }
    }
}