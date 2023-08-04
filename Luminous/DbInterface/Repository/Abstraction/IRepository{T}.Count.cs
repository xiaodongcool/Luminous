using System.Linq.Expressions;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     仓储接口
    /// </summary>
    public partial interface IRepository<T>
    {
        /// <summary>
        ///     统计行数
        /// </summary>
        Task<int> Count();

        /// <summary>
        ///     统计行数
        /// </summary>
        Task<int> Count(Expression<Func<T, bool>> predicate);
    }
}
