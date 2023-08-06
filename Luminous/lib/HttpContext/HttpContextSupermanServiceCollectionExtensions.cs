using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpContextSupermanServiceCollectionExtensions
    {
        /// <summary>
        ///     添加全局模型验证失败处理
        /// </summary>
        public static void AddHttpContextSuperman(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.TryAddTransient<IHttpContextAccessorSuper, HttpContextAccessorSuper>();
        }
    }
}

