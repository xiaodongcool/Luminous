using Dapper;
using System.Linq.Expressions;
using Com.Ctrip.Framework.Apollo.Internals;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     仓储抽象类
    /// </summary>
    public abstract partial class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        public abstract Task<IList<T>> FindAll();
        public abstract Task<IList<T>> FindAll(Expression<Func<T, bool>> predicate);
        public abstract Task<IList<T>> FindAll(Expression<Func<T, bool>> predicate = null, Expression<Func<T, object>> column = null, Expression<Func<T, object>> order = null, params OrderType[] orderTypes);
    }
}
