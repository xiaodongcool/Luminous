using Microsoft.Extensions.DependencyInjection;

namespace Luminous
{
    public static class LuminousServiceExtensions
    {
        public static void AddLuminous(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IHttpContexter, HttpContexter>();
            builder.AddLuminousAssemblyMetadata();
        }

        public static void AddLuminous(this WebApplicationBuilder builder, Predicate<string> assemblyNamePredicate)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IHttpContexter, HttpContexter>();
            builder.AddLuminousAssemblyMetadata(assemblyNamePredicate);
        }
    }
}
