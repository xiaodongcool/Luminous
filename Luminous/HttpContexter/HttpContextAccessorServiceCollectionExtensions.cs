using Microsoft.Extensions.DependencyInjection;

namespace Luminous
{
    public static class HttpContextAccessorServiceCollectionExtensions
    {
        public static void AddLuminousHttpContexter(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<ILuminousHttpContexter, LuminousHttpContexter>();
        }
    }
}
