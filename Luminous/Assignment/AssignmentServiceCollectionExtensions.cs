namespace Microsoft.Extensions.DependencyInjection
{
    public static class AssignmentServiceCollectionExtensions
    {
        public static void AddAssignment(this IServiceCollection services)
        {
            services.AddHttpContextSuperman();
            services.AddScoped<IPagingAssignment, PagingAssignment>();
            services.AddScoped<IQueryAssignment, QueryAssignment>();
            services.AddSingleton(new PagingAssignmentOptions());
            services.AddSingleton(new QueryAssignmentOptions());
        }
    }
}
