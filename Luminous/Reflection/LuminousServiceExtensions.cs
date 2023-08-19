using Microsoft.Extensions.DependencyInjection;

namespace Luminous
{
    public static class LuminousServiceExtensions
    {
        public static void AddLuminous(this WebApplicationBuilder builder)
        {
            builder.AddLuminousAssemblyMetadata();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IHttpContexter, HttpContexter>();
        }

        public static void AddLuminous(this WebApplicationBuilder builder, Predicate<string> assemblyNamePredicate)
        {
            builder.AddLuminousAssemblyMetadata(assemblyNamePredicate);
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IHttpContexter, HttpContexter>();
        }
    }
}
