using Microsoft.Extensions.DependencyInjection;

namespace LangM.AspNetCore
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ApiController : ControllerBase
    {
        private IServiceProvider _serviceProvider;
        private IUserPrincipal _user;

        /// <summary>
        ///     服务容器
        /// </summary>
        protected IServiceProvider ServiceProvider => _serviceProvider ??= HttpContext.RequestServices;

        /// <summary>
        ///     用户信息
        /// </summary>
        protected new IUserPrincipal User => _user ??= ServiceProvider.GetService<IUserPrincipal>();

        public ApiController() { }

        /// <summary>
        ///     设置响应报文的 <see cref="IContact{T}.Message"/> 字段
        /// </summary>
        protected string Message
        {
            set => HttpContext.SetMessage(value);
        }

        /// <summary>
        ///     设置响应报文的 <see cref="IContact{T}.Status"/> 字段
        /// </summary>
        protected WebApiStatusCode Status
        {
            set => HttpContext.SetCode(value);
        }
    }
}
