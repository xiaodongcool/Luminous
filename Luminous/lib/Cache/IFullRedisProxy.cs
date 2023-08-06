using NewLife.Caching;

namespace Luminous
{
    /// <summary>
    ///     <see cref="FullRedis"/> 代理接口
    /// </summary>
    public interface IFullRedisProxy : IRedis
    {
        FullRedis Client { get; }
    }
}
