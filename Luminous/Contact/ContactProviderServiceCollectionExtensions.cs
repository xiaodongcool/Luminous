using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ContactProviderServiceCollectionExtensions
    {
        public static void AddContactProvider(this IServiceCollection services)
        {
            services.TryAddSingleton<IResultFactory, DefaultResultFactory>();
        }
    }
}
