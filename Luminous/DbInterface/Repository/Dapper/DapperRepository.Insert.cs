using System.Diagnostics;
using Dapper;

namespace LangM.AspNetCore.DbInterface
{
    public abstract partial class DapperRepository<T> : IDapperRepository<T> where T : class, IEntity, new()
    {
        public override async Task<int> Insert(T entity)
        {
            if (entity.Id == 0)
            {
                entity.Id = _workId.Next();
            }

            if (entity is ICreateUpdateTimeProperties entity2)
            {
                if (entity2.CreateTime == default)
                {
                    entity2.CreateTime = DateTime.Now;
                }
                if (entity2.UpdateTime == default)
                {
                    entity2.UpdateTime = DateTime.Now;
                }
            }

            var options = BuildSqlOptions();

            var ctx = _sql.Insert(options, entity);

            var connection = await ParseConnection(ctx);

            Debug.Assert(connection != null);

            return await connection.ExecuteAsync(ctx.Sql, ctx.Parameters);
        }

        public override async Task<int> Insert(IList<T> entities)
        {
            var sum = 0;
            foreach (var entity in entities)
            {
                sum += await Insert(entity);
            }

            return sum;
        }
    }
}
