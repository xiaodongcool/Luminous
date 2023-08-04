using System.Linq.Expressions;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     仓储接口
    /// </summary>
    public partial interface IRepository<T>
    {
        /// <summary>
        ///     删除
        /// </summary>
        Task<int> Delete(params long[] idArray);

        /// <summary>
        ///     删除
        /// </summary>
        Task<int> Delete(Expression<Func<T, bool>> predicate);
    }
}
