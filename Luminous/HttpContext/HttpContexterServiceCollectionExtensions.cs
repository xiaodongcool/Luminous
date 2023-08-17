using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Luminous
{
    public static class HttpContexterServiceCollectionExtensions
    {
        /// <summary>
        ///     添加全局模型验证失败处理
        /// </summary>
        public static void AddLuminousHttpContexter(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.TryAddTransient<IHttpContexter, HttpContexter>();
        }
    }
}

