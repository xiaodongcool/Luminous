using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     Dapper 仓储抽象类
    /// </summary>
    public abstract partial class DapperRepository<T> : Repository<T>, IDapperRepository<T> where T : class, IEntity, new()
    {
        private readonly IWorkId _workId;
        private readonly IDapperUnitOfWork _unitOfWork;
        private readonly ISql<T> _sql;

        protected override IDbConnection DbConnection => SharedDb();

        protected IDbConnection SharedDb(T entity = null) => _unitOfWork.GetConnection(GetDbName(false), GetDbSuffixFinal(entity)).Connection;
        protected IDbConnection SharedDb(string suffix) => _unitOfWork.GetConnection(GetDbName(false), suffix).Connection;

        protected DapperRepository(
            IWorkId workId,
            IDapperUnitOfWork unitOfWork,
            IShardEnsured shardStrategy,
            IEntityReflection<T> entityReflection,
            ISql<T> sqlStatement
        ) : base(shardStrategy, sqlStatement, entityReflection)
        {
            _workId = workId;
            _unitOfWork = unitOfWork;
            _sql = sqlStatement;
        }

        public override string GetTbName(bool withSuffix = true)
        {
            var name = GetAttribute<TableAttribute>()?.Name ?? typeof(T).Name;
            if (withSuffix)
            {
                return name + GetTbSuffixFinal();
            }
            else
            {
                return name;
            }
        }

        public override string GetDbName(bool withSuffix = true)
        {
            var name = GetAttribute<DbAttribute>()?.DbName ?? throw new Exception($"实体 {typeof(T).FullName} 未定义库名,请通过 DbAttribute 定义");
            if (withSuffix)
            {
                return name + GetDbSuffixFinal();
            }
            else
            {
                return name;
            }
        }

        public virtual async Task<IDbConnection> ParseConnection(SqlContext ctx)
        {
            Debug.Assert(ctx != null);

            if (ctx.EnsureStatus == SharedEnsureStatus.Sharding)
            {
                await EnsureShared(ctx);

                if (ctx.EnsureStatus == SharedEnsureStatus.NotCreate)
                {
                    return null;
                }
            }

            return SharedDb(ctx.SharedDb);
        }
    }
}
