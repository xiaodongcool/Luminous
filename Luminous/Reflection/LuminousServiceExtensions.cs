using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace Luminous
{
    public static class LuminousServiceExtensions
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

        public static void AddLuminousCore(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IHttpContexter, HttpContexter>();
        }
    }
}
