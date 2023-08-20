using AutoMapper;
using Luminous;
using Microsoft.AspNetCore.Mvc;
using ServiceA.WebApi.ServiceInvocation;

namespace ServiceA.WebApi.Controllers
{
    public class HomeController : LuminousController
    {
        public Task<object> Index() => LuminousUtil.GetStatus();
    }
    public class UserController : LuminousController
    {
        private readonly IMapper mapper;
        private readonly IAuthenticationWebApi authenticationWebApi;

        public UserController(IMapper mapper, IAuthenticationWebApi authenticationWebApi)
        {
            this.mapper = mapper;
            this.authenticationWebApi = authenticationWebApi;
        }

        [HttpPost]
        public async Task<LoginResponse?> Login([FromBody] LoginRequest request)
        {
            var user = await authenticationWebApi.Login(request);
            return user;
        }
    }
}