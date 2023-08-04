using System.Data;
using LangM.AspNetCore.Model;
using SqlSugar;
using System.Linq.Expressions;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     SqlSugar 仓储基类
    /// </summary>
    public abstract class SqlSugarRepository<T> : Repository<T>, ISqlSugarRepository<T> where T : class, IEntity, new()
    {
        private readonly IWorkId _workId;
        private readonly ISqlSugarUnitOfWork _unitOfWork;

        protected override IDbConnection DbConnection => SplitDb().Ado.Connection;
        protected SqlSugarClient SplitDb(T entity = null) => _unitOfWork.GetConnection(GetDbName(false), GetDbSuffixFinal(entity));

        protected SqlSugarRepository(
            IWorkId workId,
            ISqlSugarUnitOfWork unitOfWork,
            IShardEnsured shardStrategy,
            ISharedContainer sharedContainer
        ) : base(shardStrategy, sharedContainer)
        {
            _workId = workId;
            _unitOfWork = unitOfWork;
        }

        private List<IConditionalModel> GetDelConditions()
        {
            var conditions = new List<IConditionalModel>();

            if (IsFullEntity())
            {
                conditions.Add(new ConditionalModel
                {
                    FieldName = "delete_flag",
                    FieldValue = "0"
                });
            }

            return conditions;
        }

        public override string GetTbName(bool withSuffix = true)
        {
            var name = GetAttribute<SugarTable>()?.TableName ?? throw new Exception("请使用 SugarTableAttribute 指定表名");

            if (withSuffix)
            {
                return name + GetTbSuffixFinal();
            }
            else
            {
                return name;
            }
        }

        public string GetTableName(T entity)
        {
            return GetTbName(false) + GetTbSuffixFinal(entity);
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

        public override async Task Insert(T entity)
        {
            if (entity.Id == 0)
            {
                entity.Id = _workId.Next();
            }

            if (entity is FullEntity entity2)
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

            await SplitDb(entity).Insertable(entity).AS(GetTableName(entity)).ExecuteCommandAsync();
        }

        public override async Task Delete(long id)
        {
            if (IsFullEntity())
            {
                var item = await SplitDb().Queryable<T>().AS(GetTbName()).FirstAsync(_ => _.Id == id);

                if (item != null)
                {
                    ReflectionCache.SetValue(item, "DeleteFlag", true);
                    await SplitDb().Updateable(item).AS(GetTbName()).ExecuteCommandAsync();
                }
            }
            else
            {
                throw new NotSupportedException("不支持软删除方法");
            }
        }

        public override async Task PhysicallyDelete(long id)
        {
            var item = await SplitDb().Queryable<T>().AS(GetTbName()).FirstAsync(_ => _.Id == id);
            if (item != null)
            {
                await SplitDb().Deleteable(item).AS(GetTbName()).ExecuteCommandAsync();
            }
        }

        public override async Task<T> Find(long id)
        {
            var item = await SplitDb().Queryable<T>().AS(GetTbName()).Where(_ => _.Id == id).FirstAsync();

            if (item == null)
            {
                return null;
            }

            if (item is FullEntity { DeleteFlag: true })
            {
                return null;
            }

            return item;
        }

        public override async Task Update(T entity)
        {
            await SplitDb(entity).Updateable(entity).AS(GetTableName(entity)).ExecuteCommandAsync();
        }

        public override async Task<IPage<T>> FindPage()
        {
            var total = new RefAsync<int>(0);
            var list = await SplitDb().Queryable<T>().AS(GetTbName()).Where(GetDelConditions()).OrderByIF(IsFullEntity(), "create_time desc,id desc").ToPageListAsync(_page.PageIndex, _page.PageSize, total);
            return new Page<T>(list, total.Value);
        }

        public async Task<IPage<T>> GetPage(Expression<Func<T, T>> select)
        {
            var total = new RefAsync<int>(0);
            var list = await SplitDb().Queryable<T>().AS(GetTbName()).Where(GetDelConditions()).Select(select).OrderByIF(IsFullEntity(), "create_time desc,id desc").ToPageListAsync(_page.PageIndex, _page.PageSize, total);
            return new Page<T>(list, total.Value);
        }

        public override async Task<IList<T>> FindAll()
        {
            if (IsFullEntity())
            {
                return await SplitDb().Queryable<T>().AS(GetTbName()).Where(GetDelConditions()).ToListAsync();
            }
            else
            {
                return await SplitDb().Queryable<T>().AS(GetTbName()).ToListAsync();
            }
        }
    }
}
