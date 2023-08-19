using Microsoft.Extensions.DependencyInjection;

namespace Luminous
{
    public static class LuminousServiceCollectionExtensions
    {
        public static void AddLuminous(this WebApplicationBuilder builder)
        {
            builder.AddLuminousAssemblyMetadata();
            builder.AddLuminousCore();
        }

        public static void AddLuminous(this WebApplicationBuilder builder, Predicate<string> assemblyNamePredicate)
        {
            builder.AddLuminousAssemblyMetadata(assemblyNamePredicate);
            builder.AddLuminousCore();
        }

        private static void AddLuminousCore(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IRelection, DefaultRelection>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ILuminousHttpContexter, LuminousHttpContexter>();
        }
    }
}
