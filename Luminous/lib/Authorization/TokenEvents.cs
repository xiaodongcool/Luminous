using Luminous.HttpContext;
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
        private ILogger<TokenEvents> _logger = null!;
        private IHttpContexter _httpContexter = null!;

        private void Initialization(HttpContext httpContext)
        {
            _logger = httpContext.RequestServices.GetRequiredService<ILogger<TokenEvents>>();
            _httpContexter = httpContext.RequestServices.GetRequiredService<IHttpContexter>();
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
            _logger.LogWarning($"403Forbidden,url:{_httpContexter.GetHttpRequestFeature().Path}{Environment.NewLine}header:{JsonConvert.SerializeObject(context.HttpContext.Request.Headers)}{Environment.NewLine}body:{await _httpContexter.GetBody()}");

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
            await httpResponse.WriteAsync(JsonConvert.SerializeObject(response, Global.JsonSerializerSettings));
        }
    }
}
