using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NewLife.Caching;

namespace Luminous
{
    public static class FullRedisDistributedCacheServiceCollectionExtensions
    {
        public static void AddLuminousRedis(this IServiceCollection services, RedisOptions options = null)
        {
            options ??= LuminousConfiguration.Redis;

            var fullRedis = new FullRedis { Tracer = null, Timeout = options.Timeout };
            fullRedis.Init($"server={options.Server}:{options.Port};db={options.Db}");

            var fullRedisDistributedCache = new FullRedisDistributedCache(fullRedis);

            services.TryAddSingleton<IRedis>(fullRedisDistributedCache);
            services.TryAddSingleton<IFullRedisProxy>(fullRedisDistributedCache);
        }
    }
}
