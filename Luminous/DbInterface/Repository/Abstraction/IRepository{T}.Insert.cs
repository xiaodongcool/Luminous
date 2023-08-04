namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     仓储接口
    /// </summary>
    public partial interface IRepository<T>
    {
        /// <summary>
        ///     插入
        /// </summary>
        Task<int> Insert(T entity);

        /// <summary>
        ///     批量插入
        /// </summary>
        Task<int> Insert(IList<T> entity);
    }
}
