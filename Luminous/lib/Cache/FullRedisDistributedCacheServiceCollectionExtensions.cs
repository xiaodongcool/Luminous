using Microsoft.Extensions.DependencyInjection.Extensions;
using NewLife.Caching;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FullRedisDistributedCacheServiceCollectionExtensions
    {
        public static void AddLuminousRedis(this IServiceCollection services, RedisOptions options = null)
        {
            options ??= CONFIGS.Redis;

            var fullRedis = new FullRedis { Tracer = null, Timeout = options.Timeout };
            fullRedis.Init($"server={options.Server}:{options.Port};db={options.Db}");

            var fullRedisDistributedCache = new FullRedisDistributedCache(fullRedis);

            services.TryAddSingleton<IRedis>(fullRedisDistributedCache);
            services.TryAddSingleton<IFullRedisProxy>(fullRedisDistributedCache);
        }
    }
}
