using Luminous;

namespace ServiceA.WebApi.ServiceInvocation
{
    [ServiceName("authentication-web-api")]
    public interface IAuthenticationWebApi : IServiceInvocation
    {
        [Post("/Authentication/Login")]
        Task<LoginResponse> Login(LoginRequest request);
    }
}
