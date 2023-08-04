using Dapper;
using System.Linq.Expressions;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     Dapper 仓储抽象类
    /// </summary>
    public abstract partial class DapperRepository<T>
    {
        public override Task<int> Delete(params long[] idArray)
        {
            if (idArray.Length == 1)
            {
                return Delete(_ => _.Id == idArray[0]);
            }
            else
            {
                return Delete(_ => idArray.Contains(_.Id));
            }
        }

        public override async Task<int> Delete(Expression<Func<T, bool>> predicate)
        {
            var options = BuildSqlOptions();

            var ctx = _sql.Delete(options, predicate);

            var connection = await ParseConnection(ctx);


            if (connection != null)
            {
                return await connection.ExecuteAsync(ctx.Sql, ctx.Parameters);
            }
            else
            {
                //  TODO:返回影响行数
                return 0;
            }
        }
    }
}
