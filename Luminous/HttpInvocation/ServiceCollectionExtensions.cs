using Microsoft.Extensions.DependencyInjection;

namespace Luminous
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        ///     添加HTTP调用
        /// </summary>
        public static void AddLuminousHttpInvocation(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IHttpInvocation, HttpInvocation>();
            services.AddSingleton<IDynamicHttpInvocation, DynamicHttpInvocation>();
        }
    }
}
