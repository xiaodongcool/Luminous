using System.Linq.Expressions;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     仓储接口
    /// </summary>
    public partial interface IRepository<T>
    {
        /// <summary>
        ///     查询所有
        /// </summary>
        Task<IList<T>> FindAll();

        /// <summary>
        ///     查询所有
        /// </summary>
        Task<IList<T>> FindAll(Expression<Func<T, bool>> predicate);

        /// <summary>
        ///     查询所有
        ///     TODO:排序考虑默认行为
        ///     TODO:考虑支持 字段1 倒叙 字段2 正序排序
        /// </summary>
        Task<IList<T>> FindAll(Expression<Func<T, bool>> predicate = null, Expression<Func<T, object>> column = null, Expression<Func<T, object>> order = null, params OrderType[] orderTypes);
    }
}
