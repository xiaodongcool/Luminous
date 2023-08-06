using AutoMapper;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AutoMapperServiceCollectionExtensions
    {
        public static void RegisterAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(TypeContainer.FindChildClass<Profile>().ToArray());
        }
    }
}
