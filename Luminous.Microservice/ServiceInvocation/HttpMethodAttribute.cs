namespace Luminous
{
    /// <summary>
    /// 请求方法基类
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class HttpMethodAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <param name="method"></param>
        public HttpMethodAttribute(string requestUrl, HttpMethod method)
        {
            ArgumentGuard.CheckForNull(requestUrl, nameof(requestUrl));

            if (!requestUrl.StartsWith("/"))
            {
                requestUrl = "/" + requestUrl;
            }

            RequestUrl = requestUrl;
            Method = method;
        }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 请求谓词
        /// </summary>
        public HttpMethod Method { get; set; }
    }

    /// <summary>
    /// HttpGet 请求
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class GetAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="requestUrl"></param>
        public GetAttribute(string requestUrl) : base(requestUrl, HttpMethod.Get)
        {
        }
    }

    /// <summary>
    /// HttpPost 请求
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PostAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="requestUrl"></param>
        public PostAttribute(string requestUrl) : base(requestUrl, HttpMethod.Post)
        {
        }
    }
}
