using AutoMapper;
using System.Diagnostics;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AutoMapperServiceCollectionExtensions
    {
        public static void AddLuminousAutoMapper(this IServiceCollection services)
        {
            Debug.Assert(Global.Solution != null);

            services.AddAutoMapper(Global.Solution.GetDerivedClass(typeof(Profile)).ToArray());
        }
    }
}
