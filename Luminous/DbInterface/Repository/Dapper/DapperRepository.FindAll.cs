using Dapper;
using System.Data;
using System.Diagnostics;
using System.Linq.Expressions;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     Dapper 仓储抽象类
    /// </summary>
    public abstract partial class DapperRepository<T>
    {
        /// <summary>
        ///     查询所有
        /// </summary>
        public override Task<IList<T>> FindAll()
        {
            return FindAll(null, null);
        }

        /// <summary>
        ///     查询所有
        /// </summary>
        public override Task<IList<T>> FindAll(Expression<Func<T, bool>> predicate)
        {
            return FindAll(predicate, null);
        }

        public override async Task<IList<T>> FindAll(Expression<Func<T, bool>> predicate = null, Expression<Func<T, object>> column = null, Expression<Func<T, object>> order = null, params OrderType[] orderTypes)
        {
            var options = BuildSqlOptions(column, order: order, orderTypes: orderTypes);

            var ctx = _sql.Select(options, predicate);

            var connection = await ParseConnection(ctx);

            return connection != null ? (await connection.QueryAsync<T>(ctx.Sql, ctx.Parameters)).ToList() : new List<T>();
        }
    }
}
