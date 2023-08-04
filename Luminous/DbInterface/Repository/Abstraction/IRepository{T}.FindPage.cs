using LangM.AspNetCore.Model;
using System.Linq.Expressions;

namespace LangM.AspNetCore.DbInterface
{
    public partial interface IRepository<T>
    {
        /// <summary>
        ///     分页查询所有数据
        /// </summary>
        Task<IPage<T>> FindPage(int index, int size);

        /// <summary>
        ///     分页查询所有数据
        /// </summary>
        Task<IPage<T>> FindPage(int index, int size, Expression<Func<T, bool>> predicate);

        Task<IPage<T>> FindPage(int index, int size, Expression<Func<T, bool>> predicate = null, Expression<Func<T, object>> column = null, Expression<Func<T, object>> order = null, params OrderType[] orderTypes);
    }
}
