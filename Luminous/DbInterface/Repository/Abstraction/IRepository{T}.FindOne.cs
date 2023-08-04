using System.Linq.Expressions;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     仓储接口
    /// </summary>
    public partial interface IRepository<T>
    {
        /// <summary>
        ///     根据主键查询
        /// </summary>
        Task<T> Find(long id);

        /// <summary>
        ///     查询满足条件的第一条记录
        /// </summary>
        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);
    }
}
