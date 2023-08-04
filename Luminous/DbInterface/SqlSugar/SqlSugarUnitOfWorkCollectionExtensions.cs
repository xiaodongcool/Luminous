using LangM.AspNetCore.DbInterface;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SqlSugarUnitOfWorkCollectionExtensions
    {
        public static void AddSqlSugarUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<ISqlSugarUnitOfWork, SqlSugarUnitOfWork>();
        }
    }
}
