using Microsoft.Extensions.DependencyInjection;

namespace Luminous
{
    /// <summary>
    ///     静态容器
    /// </summary>
    public static class RequestServices
    {
        public static IServiceProvider GetServiceProvider() => new HttpContextAccessor().HttpContext?.RequestServices;
        public static T GetService<T>() => GetServiceProvider().GetService<T>();
    }
}
