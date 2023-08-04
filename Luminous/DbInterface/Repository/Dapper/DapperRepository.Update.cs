using Dapper;
using System.Linq.Expressions;

namespace LangM.AspNetCore.DbInterface
{
    public abstract partial class DapperRepository<T> : IDapperRepository<T> where T : class, IEntity, new()
    {
        public override Task<int> Update(T entity)
        {
            return Update(entity, null);
        }

        public override async Task<int> Update(IList<T> entities)
        {
            var sum = 0;

            foreach (var entity in entities)
            {
                sum += await Update(entity);
            }

            return sum;
        }

        public override Task<int> Update(T entity, Expression<Func<T, bool>> predicate)
        {
            return Update(entity, predicate, null);
        }

        public override Task<int> Update(T entity, Expression<Func<T, object>> column)
        {
            return Update(entity, null, column);
        }

        public override async Task<int> Update(IList<T> entities, Expression<Func<T, object>> column)
        {
            var sum = 0;

            foreach (var entity in entities)
            {
                sum += await Update(entity, column);
            }

            return sum;
        }

        public override async Task<int> Update(T entity, Expression<Func<T, bool>> predicate, Expression<Func<T, object>> column)
        {
            var options = BuildSqlOptions(updateColumn: column);

            var ctx = _sql.Update(options, entity, predicate);

            var connection = await ParseConnection(ctx);

            if (connection != null)
            {
                return await connection.ExecuteAsync(ctx.Sql, ctx.Parameters);
            }
            else
            {
                return 0;
            }
        }
    }
}
