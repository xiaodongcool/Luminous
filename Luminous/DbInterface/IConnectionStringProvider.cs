namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     数据库连接字符串接口
    /// </summary>
    public interface IConnectionStringProvider
    {
        /// <summary>
        ///     获取数据库连接字符串
        /// </summary>
        string Get(string dbName, string split);
        /// <summary>
        ///     默认数据库
        /// </summary>
        string DefaultDb { get; }
    }
}
