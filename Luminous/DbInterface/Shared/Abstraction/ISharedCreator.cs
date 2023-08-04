namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     创建分片
    /// </summary>
    public interface ISharedCreator
    {
        /// <summary>
        ///     创建分库
        /// </summary>
        Task CreateDb(string dbName, string newDbName);
        /// <summary>
        ///     创建分表
        /// </summary>
        Task CreateTable(string dbName, string tableName, string newTableName);
        /// <summary>
        ///     判断数据库是否存在
        /// </summary>
        Task<bool> ExistsDb(string dbName);
        /// <summary>
        ///     判断表是否存在
        /// </summary>
        Task<bool> ExistsTable(string dbName, string tbName);
    }
}
