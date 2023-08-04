using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace Luminous
{
    public class HttpContextAccessorSuper : HttpContextAccessor, IHttpContextAccessorSuper
    {
        private readonly ILogger<HttpContextAccessorSuper> _logger;

        public HttpContextAccessorSuper(ILogger<HttpContextAccessorSuper> logger)
        {
            _logger = logger;
        }

        public string GetAuthorizationBearer()
        {
            var headers = HttpContext?.Request?.Headers;

            if (headers == null)
            {
                return "";
            }

            if (!headers.ContainsKey(HeaderNames.Authorization))
            {
                return "";
            }

            string authorization = headers[HeaderNames.Authorization];

            if (string.IsNullOrEmpty(authorization))
            {
                return "";
            }

            if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return authorization.Substring("Bearer ".Length).Trim();
            }
            else
            {
                return "";
            }
        }

        public async Task<string> GetBody()
        {
            if (HttpContext?.Request?.Body == null)
            {
                return "";
            }

            const string key = "_http_body_";

            var body = HttpContext.Items[key]?.ToString();

            if (string.IsNullOrEmpty(body))
            {
                try
                {
                    var request = HttpContext.Request;

                    request.EnableBuffering();

                    using var reader = new StreamReader(request.Body, Encoding.UTF8, false, -1, true);

                    body = await reader.ReadToEndAsync();

                    request.Body.Position = 0;

                    HttpContext.Items[key] = body;

                    return body;
                }
                catch (Exception e)
                {
                    _logger.LogError("读取 HttpRequest.Body 失败", e);
                    return "";
                }
            }

            return body;
        }

        public string GetDomain()
        {
            if (HttpContext?.Request == null)
            {
                return "";
            }

            return HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
        }

        public string GetIpAddress()
        {
            if (HttpContext == null)
            {
                return "";
            }

            //  从代理服务器获取 ip 地址

            var proxifiedIpList = HttpContext.Request?.Headers["X-forwarded-for"].FirstOrDefault();

            if (!string.IsNullOrEmpty(proxifiedIpList))
            {
                var strArray = proxifiedIpList.Split(',');
                if (strArray.Length == 0)
                {
                    return default;
                }
                else
                {
                    return !strArray[0].Contains(":") ? strArray[0] : strArray[0].Substring(0, strArray[0].LastIndexOf(":", StringComparison.Ordinal));
                }
            }

            try
            {
                //  获取客户端 ip 地址

                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
            catch (NullReferenceException)
            {
                //  在做 unit test 时, RemoteIpAddress 可能是 null , 这里提供一种方式允许并且只允许在做 unit test 时, 可以指定 ip 地址

                if (!UnitTestDetector.RunningFromUnitTest)
                {
                    return default;
                }

                var headers = HttpContext.Request?.Headers;
                if (headers?.ContainsKey("unittest-customer-ip") == true)
                {
                    return headers["unittest-customer-ip"];
                }
                else
                {
                    return "0.0.0.1";
                }
            }
        }

        public IHttpRequestFeature GetHttpRequestFeature()
        {
            return HttpContext?.Features?.Get<IHttpRequestFeature>();
        }
    }
}
