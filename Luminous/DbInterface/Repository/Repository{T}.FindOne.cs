using System.Linq.Expressions;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     仓储抽象类
    /// </summary>
    public abstract partial class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        public abstract Task<T> Find(long id);

        public abstract Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);
    }
}
   