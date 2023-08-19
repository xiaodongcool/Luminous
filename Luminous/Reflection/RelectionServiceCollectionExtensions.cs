using Microsoft.Extensions.DependencyInjection;

namespace Luminous
{
    public static class RelectionServiceCollectionExtensions
    {
        public static void AddRelection(this IServiceCollection services)
        {
            services.AddSingleton<IRelection, DefaultRelection>();
        }
    }
}
