using NewLife.Caching;

namespace LangM.AspNetCore
{
    /// <summary>
    ///     <see cref="FullRedis"/> 代理接口
    /// </summary>
    public interface IFullRedisProxy : IRedis
    {
        FullRedis Client { get; }
    }
}
