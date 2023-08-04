using LangM.AspNetCore.DbInterface;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConnectionStringCollectionExtensions
    {
        public static void AddConnectionString<TConnectionStringProvider>(this IServiceCollection services) where TConnectionStringProvider : class, IConnectionStringProvider
        {
            services.AddScoped<IConnectionStringProvider, TConnectionStringProvider>();
            services.AddSingleton<IShardEnsured, ShardEnsured>();
            services.AddSingleton<ISharedCreator, MySqlSharedCreator>();
        }
    }
}
