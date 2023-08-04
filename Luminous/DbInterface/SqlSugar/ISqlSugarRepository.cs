using LangM.AspNetCore.Model;
using System.Linq.Expressions;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     SqlSugar 仓储接口
    /// </summary>
    public interface ISqlSugarRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        Task<IPage<T>> GetPage(Expression<Func<T, T>> select);
    }
}
