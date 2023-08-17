using Luminous.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Luminous
{
    public static class RequiredServiceCollectionExtensions
    {
        /// <summary>
        ///     添加必备的一些内置服务
        /// </summary>
        public static void AddRequired(this IServiceCollection services)
        {
            services.AddSingleton<IRelection, DefaultRelection>();
        }
    }
}
