using Dapper;
using LangM.AspNetCore.Model;
using System.Linq.Expressions;

namespace LangM.AspNetCore.DbInterface
{
    public abstract partial class DapperRepository<T> : IDapperRepository<T> where T : class, IEntity, new()
    {
        /// <summary>
        ///     分页查询
        /// </summary>
        public override Task<IPage<T>> FindPage(int index, int size)
        {
            return FindPage(index, size, null);
        }

        /// <summary>
        ///     分页查询
        /// </summary>
        public override Task<IPage<T>> FindPage(int index, int size, Expression<Func<T, bool>> predicate)
        {
            return FindPage(index, size, predicate, null);
        }

        /// <summary>
        ///     分页查询
        /// </summary>
        public override async Task<IPage<T>> FindPage(int index, int size, Expression<Func<T, bool>> predicate = null, Expression<Func<T, object>> column = null, Expression<Func<T, object>> order = null, params OrderType[] orderTypes)
        {
            var options = BuildSqlOptions(column, order: order, orderTypes: orderTypes);

            options.PageIndex = index;
            options.PageSize = size;

            var ctx1 = _sql.Select(options, predicate);
            var ctx2 = _sql.Counts(options, predicate);

            var connection = await ParseConnection(ctx1);

            if (connection != null)
            {
                var data = (await connection.QueryAsync<T>(ctx1.Sql, ctx1.Parameters)).ToList();
                var count = await connection.ExecuteScalarAsync<int>(ctx2.Sql, ctx2.Parameters);

                return Page.Create(data, count);
            }
            else
            {
                return Page.Create(new List<T>(), 0);
            }
        }
    }
}
