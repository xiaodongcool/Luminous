using Dapper;
using LangM.AspNetCore.Model;
using System.Linq.Expressions;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     仓储抽象类
    /// </summary>
    public abstract partial class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        /// <summary>
        ///     分页查询所有数据
        /// </summary>
        public abstract Task<IPage<T>> FindPage(int index, int size);

        /// <summary>
        ///     分页查询所有数据
        /// </summary>
        public abstract Task<IPage<T>> FindPage(int index, int size, Expression<Func<T, bool>> predicate);

        public abstract Task<IPage<T>> FindPage(int index, int size, Expression<Func<T, bool>> predicate = null, Expression<Func<T, object>> column = null, Expression<Func<T, object>> order = null, params OrderType[] orderTypes);
    }
}
