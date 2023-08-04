using Dapper;
using System.Linq.Expressions;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     Dapper 仓储抽象类
    /// </summary>
    public abstract partial class DapperRepository<T> : IDapperRepository<T> where T : class, IEntity, new()
    {
        /// <summary>
        ///     查询所有
        /// </summary>
        public override Task<int> Count()
        {
            return Count(null);
        }

        /// <summary>
        ///     查询所有
        /// </summary>
        public override async Task<int> Count(Expression<Func<T, bool>> predicate)
        {
            var options = BuildSqlOptions();

            var ctx = _sql.Counts(options, predicate);

            var connection = await ParseConnection(ctx);

            return connection != null ? await connection.ExecuteScalarAsync<int>(ctx.Sql, ctx.Parameters) : 0;
        }
    }
}
