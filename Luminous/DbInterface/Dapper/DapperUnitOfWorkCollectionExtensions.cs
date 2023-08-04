using LangM.AspNetCore.DbInterface;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DapperUnitOfWorkCollectionExtensions
    {
        public static void AddDapperUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IDapperUnitOfWork, DapperUnitOfWork>();
        }
    }
}
