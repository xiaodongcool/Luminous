using System.Linq.Expressions;
using Dapper;

namespace LangM.AspNetCore.DbInterface
{
    public abstract partial class DapperRepository<T> : IDapperRepository<T> where T : class, IEntity, new()
    {
        public override Task<T> Find(long id)
        {
            return FirstOrDefault(_ => _.Id == id);
        }

        public override async Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            var options = BuildSqlOptions();

            options.Top = 1;

            var ctx = _sql.Select(options, predicate);

            var connection = await ParseConnection(ctx);

            return connection != null ? await connection.QueryFirstOrDefaultAsync<T>(ctx.Sql, ctx.Parameters) : null;
        }
    }
}
