namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     确保已经分片完成
    /// </summary>
    public interface IShardEnsured
    {
        /// <summary>
        ///     确保已分库
        /// </summary>
        Task<SharedEnsureStatus> EnsureDb(string dbName, string suffix, bool create);
        /// <summary>
        ///     确保已分表
        /// </summary>
        Task<SharedEnsureStatus> EnsureTb(string dbName, string tbName, string suffix, bool create);
    }
}
