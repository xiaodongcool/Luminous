using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;

namespace LangM.AspNetCore
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IContactProvider _contactProvider;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessorSuper _httpContextAccessorSuper;
        private readonly IGlobalSerializer _globalSerializer;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment env,
            IContactProvider contactProvider,
            IConfiguration configuration,
            IHttpContextAccessorSuper httpContextAccessorSuper,
            IGlobalSerializer globalSerializer)
        {
            _next = next;
            _logger = logger;
            _env = env;
            _contactProvider = contactProvider;
            _configuration = configuration;
            _httpContextAccessorSuper = httpContextAccessorSuper;
            _globalSerializer = globalSerializer;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                try
                {
                    await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.LogError(ex, nameof(ExceptionMiddleware) + e);
                }
            }
        }

        /// <summary>
        ///     处理异常
        /// </summary>
        protected virtual async Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        {
            if (exception is not ForegoneException interrupException || interrupException.CatchGlobalException)
            {
                var request = context.Request;

                var requestFeature = request.HttpContext.Features.Get<IHttpRequestFeature>();

                var body = await _httpContextAccessorSuper.GetBody();

                //  日志
                _logger.LogError(exception, $"500InternalServerError,url:{requestFeature.RawTarget}{Environment.NewLine}header:{GetHeader(requestFeature.Headers)}{Environment.NewLine}body:{body}");

            }

            //  响应报文
            var response = CreateResponseModel(exception);

            //  序列化
            var responseString = _globalSerializer.SerializeObject(response);

            //  写入 Response
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(responseString);
        }

        /// <summary>
        ///     获取请求头内容
        /// </summary>
        protected string GetHeader(IHeaderDictionary headerDictionary)
        {
            if (headerDictionary == null)
            {
                return null;
            }

            var sb = new StringBuilder();

            sb.AppendLine();

            foreach (var (name, value) in headerDictionary)
            {
                sb.AppendLine($"{name}:{value}");
            }

            return sb.ToString();
        }

        /// <summary>
        ///     创建响应报文
        /// </summary>
        protected IContact<object> CreateResponseModel(Exception exception)
        {
            IContact<object> convention;

            if (exception is ForegoneException interrupException)
            {
                convention = _contactProvider.Create<object>(interrupException.StatusCode, null, interrupException.ApiMessage);

                if (_env.IsDevelopment() || _configuration["env"] != "product")
                {
                    convention.Exception = interrupException;
                }
            }
            else
            {
                convention = _contactProvider.Create<object>(WebApiStatusCode.InternalServerError, null, "抱歉，服务器刚刚开小差了，请稍后再试。");

                if (_env.IsDevelopment() || _configuration["env"] != "product")
                {
                    convention.Error = exception.Message;
                    convention.Exception = exception;
                }
            }

            return convention;
        }
    }
}
