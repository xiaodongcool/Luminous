using System.Linq.Expressions;

namespace LangM.AspNetCore.DbInterface
{
    public interface ISql<T> where T : IEntity
    {
        //  TODO:考虑分库分表
        SqlContext Select(SqlOptions options, Expression<Func<T, bool>> predicate);
        SqlContext Counts(SqlOptions options, Expression<Func<T, bool>> predicate);
        SqlContext Insert(SqlOptions options, T entity);
        SqlContext Update(SqlOptions options, T entity, Expression<Func<T, bool>> predicate);
        SqlContext Delete(SqlOptions options, Expression<Func<T, bool>> predicate);
    }
}
