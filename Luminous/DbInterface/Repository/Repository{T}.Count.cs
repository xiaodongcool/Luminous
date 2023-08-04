using System.Linq.Expressions;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     仓储抽象类
    /// </summary>
    public abstract partial class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        /// <summary>
        ///     统计行数
        /// </summary>
        public abstract Task<int> Count();

        /// <summary>
        ///     统计行数
        /// </summary>
        public abstract Task<int> Count(Expression<Func<T, bool>> predicate);
    }
}
