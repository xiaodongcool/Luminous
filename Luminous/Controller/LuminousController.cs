﻿namespace Luminous
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LuminousController : ControllerBase
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
        //protected new IUserPrincipal User => _user ??= ServiceProvider.GetService<IUserPrincipal>();

        public LuminousController() { }

        protected void SetResultStatus(ResultStatus statusCode)
        {
            HttpContext.SetCode(statusCode);
        }

        protected void SetResultMessage(string message)
        {
            HttpContext.SetMessage(message);
        }

        protected void SetResultStatusAndMessage(ResultStatus statusCode, string message)
        {
            HttpContext.SetCode(statusCode);
            HttpContext.SetMessage(message);
        }
    }
}
