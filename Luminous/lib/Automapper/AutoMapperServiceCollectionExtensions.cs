using AutoMapper;
using Luminous.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AutoMapperServiceCollectionExtensions
    {
        public static void AddLuminousAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(TypeContainer.FindChildClass<Profile>().ToArray());
        }
    }
}
