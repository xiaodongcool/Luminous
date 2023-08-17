using Luminous.HttpContext;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AssignmentServiceCollectionExtensions
    {
        public static void AddAssignment(this IServiceCollection services)
        {
            services.AddLuminousHttpContexter();
            services.AddScoped<IPagingAssignment, PagingAssignment>();
            services.AddScoped<IQueryAssignment, QueryAssignment>();
            services.AddSingleton(new PagingAssignmentOptions());
            services.AddSingleton(new QueryAssignmentOptions());
        }
    }
}
