using Microsoft.AspNetCore.Http.Features;

namespace Luminous
{
    /// <summary>
    ///     对 HttpContext 的常见操作
    /// </summary>
    public interface ILuminousHttpContexter : IHttpContextAccessor
    {
        /// <summary>
        ///     获取请求报文
        /// </summary>
        Task<string> GetBody();

        /// <summary>
        ///     获取请求域名
        /// </summary>
        string GetDomain();

        /// <summary>
        ///     获取客户端 ip 地址
        /// </summary>
        string GetIpAddress();

        /// <summary>
        ///     获取 token
        /// </summary>
        string GetAuthorizationBearer();

        /// <summary>
        ///     获取更多的客户端请求信息
        /// </summary>
        IHttpRequestFeature GetHttpRequestFeature();
    }
}
