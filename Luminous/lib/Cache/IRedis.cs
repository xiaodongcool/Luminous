namespace Luminous
{
    public interface IRedis
    {
        string Get(string key);
        Task<string> GetAsync(string key);
        TModel Get<TModel>(string key);
        Task<TModel> GetAsync<TModel>(string key);
        void Set(string key, string value, int seconds);
        Task SetAsync(string key, string value, int seconds);
        void Set<TModel>(string key, TModel value, int seconds);
        Task SetAsync<TModel>(string key, TModel value, int seconds);
        void Del(string key);
        Task DelAsync(string key);
        void Expire(string key, int seconds);
        Task ExpireAsync(string key, int seconds);
        void Incr(string key, long value = 1);
        Task IncrAsync(string key, int value = 1);
    }
}
