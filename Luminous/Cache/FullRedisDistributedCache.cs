using NewLife.Caching;

namespace LangM.AspNetCore
{
    public class FullRedisDistributedCache : IFullRedisProxy
    {
        private readonly FullRedis _fullRedis;

        public FullRedisDistributedCache(FullRedis fullRedis)
        {
            _fullRedis = fullRedis;
        }

        public FullRedis Client => _fullRedis;

        public void Del(string key)
        {
            _fullRedis.Remove(key);
        }

        public async Task DelAsync(string key)
        {
            await Task.Run(() => Del(key));
        }

        public void Expire(string key, int seconds)
        {
            _fullRedis.SetExpire(key, TimeSpan.FromSeconds(seconds));
        }

        public async Task ExpireAsync(string key, int seconds)
        {
            await Task.Run(() => _fullRedis.SetExpire(key, TimeSpan.FromSeconds(seconds)));
        }

        public string Get(string key)
        {
            return _fullRedis.Get<string>(key);
        }

        public TModel Get<TModel>(string key)
        {
            return _fullRedis.Get<TModel>(key);
        }

        public async Task<string> GetAsync(string key)
        {
            return await Task.Run(() => _fullRedis.Get<string>(key));
        }

        public async Task<TModel> GetAsync<TModel>(string key)
        {
            return await Task.Run(() => _fullRedis.Get<TModel>(key));
        }

        public void Incr(string key, long value = 1)
        {
            _fullRedis.Increment(key, value);
        }

        public async Task IncrAsync(string key, int value = 1)
        {
            await Task.Run(() => _fullRedis.Increment(key, value));
        }

        public void Set(string key, string value, int seconds)
        {
            _fullRedis.Set(key, value, TimeSpan.FromSeconds(seconds));
        }

        public void Set<TModel>(string key, TModel value, int seconds)
        {
            _fullRedis.Set(key, value, TimeSpan.FromSeconds(seconds));
        }

        public async Task SetAsync(string key, string value, int seconds)
        {
            await Task.Run(() => _fullRedis.Set(key, value, TimeSpan.FromSeconds(seconds)));
        }

        public async Task SetAsync<TModel>(string key, TModel value, int seconds)
        {
            await Task.Run(() => _fullRedis.Set(key, value, TimeSpan.FromSeconds(seconds)));
        }
    }
}
