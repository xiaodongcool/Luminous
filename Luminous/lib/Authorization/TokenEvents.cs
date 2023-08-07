using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Luminous
{
    /// <summary>
    ///     token 授权过程中的事件
    /// </summary>
    public class TokenEvents : JwtBearerEvents
    {
        private ILogger<TokenEvents> _logger;
        private IHttpContextAccessorSuper _accessor;
        private IGlobalSerializer _globalSerializer;


        private void Initialization(HttpContext httpContext)
        {
            _logger = httpContext.RequestServices.GetService<ILogger<TokenEvents>>();
            _accessor = httpContext.RequestServices.GetService<IHttpContextAccessorSuper>();
            _globalSerializer = httpContext.RequestServices.GetService<IGlobalSerializer>();
        }

        /// <summary>
        ///     token 未通过授权
        /// </summary>
        public override async Task Challenge(JwtBearerChallengeContext context)
        {
            Initialization(context.HttpContext);

            //  跳过默认的 challenge 处理
            //  不加这行代码会报 StatusCode cannot be set because the response has already started.
            context.HandleResponse();

            var result = new Result<object>(ResultStatus.UnAuthorized, null, $"{context.Error}（{context.ErrorDescription}）");

            await WriteResponse(context.Response, HttpStatusCode.Unauthorized, result);
        }

        /// <summary>
        ///     token 通过授权
        /// </summary>
        //public override Task TokenValidated(TokenValidatedContext context)
        //{
        //    Initialization(context.HttpContext);
        //    return Task.CompletedTask;
        //}

        /// <summary>
        ///     权限不足
        /// </summary>
        public override async Task Forbidden(ForbiddenContext context)
        {
            Initialization(context.HttpContext);

            //  非法访问,记录日志
            _logger.LogWarning($"403Forbidden,url:{_accessor.GetHttpRequestFeature().Path}{Environment.NewLine}header:{Serializer(context.HttpContext.Request.Headers)}{Environment.NewLine}body:{await _accessor.GetBody()}");

            var result = new Result<object>(ResultStatus.Forbidden, null, null);

            await WriteResponse(context.Response, HttpStatusCode.Forbidden, result);
        }

        /// <summary>
        ///     向响应报文写入 http 状态码和报文
        /// </summary>
        private async Task WriteResponse<T>(HttpResponse httpResponse, HttpStatusCode httpStatusCode, IResult<T> response)
        {
            httpResponse.ContentType = "application/json; charset=utf-8";
            httpResponse.StatusCode = (int)httpStatusCode;
            await httpResponse.WriteAsync(Serializer(response));
        }

        /// <summary>
        ///     使用默认的方式序列化
        /// </summary>
        private string Serializer<T>(T response) => _globalSerializer.SerializeObject(response);
    }
}
