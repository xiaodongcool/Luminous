using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;

namespace Luminous
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessorSuper _httpContextAccessorSuper;
        private readonly IGlobalSerializer _globalSerializer;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment env,
            IConfiguration configuration,
            IHttpContextAccessorSuper httpContextAccessorSuper,
            IGlobalSerializer globalSerializer)
        {
            _next = next;
            _logger = logger;
            _env = env;
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
            if (exception is not FailException interrupException || interrupException.LogOnGlobalException)
            {
                var request = context.Request;

                var requestFeature = request.HttpContext.Features.Get<IHttpRequestFeature>();

                var body = await _httpContextAccessorSuper.GetBody();

                //  日志
                _logger.LogError(exception, $"500InternalServerError,url:{requestFeature.RawTarget}{Environment.NewLine}header:{GetHeader(requestFeature.Headers)}{Environment.NewLine}body:{body}");
            }

            //  响应报文
            var result = GetResult(exception);

            //  写入 Response
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(_globalSerializer.SerializeObject(result));
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
        protected IResult<object> GetResult(Exception exception)
        {
            IResult<object> result;

            if (exception is FailException failException)
            {
                if (_env.IsProduction())
                {
                    result = new Result<object>(failException.StatusCode, null, failException.ResultMessage);
                }
                else
                {
                    result = new DebugResult<object>(failException.StatusCode, null, failException.ResultMessage, failException, null);
                }
            }
            else
            {
                if (_env.IsProduction())
                {
                    result = new Result<object>(ResultStatus.InternalServerError, null, "抱歉，服务器刚刚开小差了，请稍后再试。");
                }
                else
                {
                    result = new DebugResult<object>(ResultStatus.InternalServerError, null, null, exception, exception.Message);
                }
            }

            return result;
        }
    }

    public interface IApplication
    {
        Env Env { get; }
    }

    public class Application : IApplication
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Application(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public Env Env
        {
            get
            {
                return Env.Development;
            }
        }
    }

    public enum Env
    {
        Development,
        Testing,
        Uat,
        Production
    }
}
