using Microsoft.Extensions.DependencyInjection;

namespace LangM.AspNetCore
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
