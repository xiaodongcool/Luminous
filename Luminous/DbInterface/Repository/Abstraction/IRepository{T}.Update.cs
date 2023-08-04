using System.Linq.Expressions;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     仓储接口
    /// </summary>
    public partial interface IRepository<T>
    {
        /// <summary>
        ///     更新
        /// </summary>
        Task<int> Update(T entity);

        /// <summary>
        ///     批量更新
        /// </summary>
        Task<int> Update(IList<T> entity);

        /// <summary>
        ///     批量更新
        /// </summary>
        Task<int> Update(T entity, Expression<Func<T, bool>> predicate);

        /// <summary>
        ///     更新
        /// </summary>
        Task<int> Update(T entity, Expression<Func<T, object>> column);

        /// <summary>
        ///     批量更新
        /// </summary>
        Task<int> Update(IList<T> entity, Expression<Func<T, object>> column);

        /// <summary>
        ///     批量更新
        /// </summary>
        Task<int> Update(T entity, Expression<Func<T, bool>> predicate, Expression<Func<T, object>> column);
    }
}
