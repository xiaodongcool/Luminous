using System.Linq.Expressions;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     仓储抽象类
    /// </summary>
    public abstract partial class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        public abstract Task<int> Update(T entity);
        public abstract Task<int> Update(IList<T> entity);
        public abstract Task<int> Update(T entity, Expression<Func<T, bool>> predicate);
        public abstract Task<int> Update(T entity, Expression<Func<T, object>> column);
        public abstract Task<int> Update(IList<T> entity, Expression<Func<T, object>> column);
        public abstract Task<int> Update(T entity, Expression<Func<T, bool>> predicate, Expression<Func<T, object>> column);
    }
}
