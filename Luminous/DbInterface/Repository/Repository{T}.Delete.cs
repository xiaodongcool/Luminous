using System.Linq.Expressions;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     仓储抽象类
    /// </summary>
    public abstract partial class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        /// <summary>
        ///     删除
        /// </summary>
        public abstract Task<int> Delete(params long[] idArray);

        /// <summary>
        ///     删除
        /// </summary>
        public abstract Task<int> Delete(Expression<Func<T, bool>> predicate);
    }
}
