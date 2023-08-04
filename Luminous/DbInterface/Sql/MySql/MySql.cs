using System.Diagnostics;
using System.Linq.Expressions;

namespace LangM.AspNetCore.DbInterface
{
    public class MySql<T> : Sql<T> where T : IEntity
    {
        public MySql(IEntityReflection<T> entityReflection) : base(entityReflection) { }

        protected override string WarpCoat(string name) => $"`{name}`";
        protected override string ParametersPrefix => "@";

        public override SqlContext Select(SqlOptions options, Expression<Func<T, bool>> predicate)
        {
            Debug.Assert(options != null);

            var ctx = new SqlContext(QueryType.Select);

            //  WHERE
            AppendWhere(ctx, predicate);

            ctx.SharedDb = CheckSuffix(Shared.Db, options.DbSuffix, ctx.SharedDb);
            ctx.SharedTb = CheckSuffix(Shared.Tb, options.TbSuffix, ctx.SharedTb);

            var tableName = GetTableName() + ctx.SharedTb;

            //  SELECT * FROM
            ctx.InsertAtStart($"SELECT {GetSelectColumnSql(options.SelectColumns)} FROM {WarpCoat(tableName)}");

            //  ORDER BY
            if (options.Orders.Length > 0)
            {
                ctx.Append(" ORDER BY");

                for (var i = 0; i < options.Orders.Length; i++)
                {
                    var order = options.Orders[i];

                    ctx.Append($" {GetPropertyMetadata(order.Column).Column} {order.SortType}");

                    if (i != options.Orders.Length - 1)
                    {
                        ctx.Append(",");
                    }
                }
            }

            //  LIMIT
            if (options.Top > 0)
            {
                ctx.Append($" LIMIT {options.Top}");
            }
            else if (options.PageIndex > 0 && options.PageSize > 0)
            {
                var skip = (options.PageIndex - 1) * options.PageSize;
                var size = options.PageSize;
                ctx.Append($" LIMIT {skip},{size}");
            }

            ctx.Logger();

            return ctx;
        }

        public override SqlContext Counts(SqlOptions options, Expression<Func<T, bool>> predicate)
        {
            Debug.Assert(options != null);

            var ctx = new SqlContext(QueryType.Select);

            //  WHERE
            AppendWhere(ctx, predicate);

            ctx.SharedDb = CheckSuffix(Shared.Db, options.DbSuffix, ctx.SharedDb);
            ctx.SharedTb = CheckSuffix(Shared.Tb, options.TbSuffix, ctx.SharedTb);

            var tableName = GetTableName() + ctx.SharedTb;

            //  SELECT COUNT(*) FROM
            ctx.InsertAtStart($"SELECT COUNT(*) FROM {WarpCoat(tableName)}");

            ctx.Logger();

            return ctx;
        }

        public override SqlContext Insert(SqlOptions options, T entity)
        {
            Debug.Assert(options != null);
            Debug.Assert(entity != null);

            var ctx = new SqlContext(QueryType.Insert)
            {
                SharedDb = CheckSuffix(Shared.Db, options.DbSuffix, null, _entityReflection.SharedDb.GetSharedSuffix(entity)),
                SharedTb = CheckSuffix(Shared.Tb, options.TbSuffix, null, _entityReflection.SharedTb.GetSharedSuffix(entity))
            };

            var tableName = GetTableName() + ctx.SharedTb;

            ctx.Append($"INSERT INTO {WarpCoat(tableName)} ({GetInsertColumnNameSql()}) values ({GetInsertColumnValueSql()})");

            ctx.Parameters = entity;

            //  TODO：看怎么做更好，定义一个 AOP 让用户自己处理
            ctx.Logger();

            return ctx;
        }

        public override SqlContext Update(SqlOptions options, T entity, Expression<Func<T, bool>> predicate)
        {
            //  TODO:更新 删除 禁止诸如 => true 的表达式
            //  TODO:指定字段更新会把 entity 所有字段都作为参数提交到服务器，应该只提交指定的字段

            Debug.Assert(options != null);
            Debug.Assert(entity != null);

            var ctx = new SqlContext(QueryType.Update);

            if (predicate == null)
            {
                if (entity.Id <= 0)
                {
                    throw new NotSetPrimaryKeyException<T>(entity);
                }

                ctx.SharedDb = CheckSuffix(Shared.Db, options.DbSuffix, null, _entityReflection.SharedDb.GetSharedSuffix(entity));
                ctx.SharedTb = CheckSuffix(Shared.Tb, options.TbSuffix, null, _entityReflection.SharedTb.GetSharedSuffix(entity));

                ctx.Append($" WHERE {GetPropertyMetadata(nameof(entity.Id)).Column} = {entity.Id}");
                ctx.Parameters = entity;
            }
            else
            {
                //  WHERE
                AppendWhere(ctx, predicate);

                var isDelete = options.UpdateColumns.Length == 1 && options.UpdateColumns[0] == nameof(ISoftDeleteProperties.DeleteFlag);

                string fromEntityDbSuffix = "", fromEntityTbSuffix = "";

                if (!isDelete)
                {
                    fromEntityDbSuffix = _entityReflection.SharedDb.GetSharedSuffix(entity);
                    fromEntityTbSuffix = _entityReflection.SharedTb.GetSharedSuffix(entity);
                }

                ctx.SharedDb = CheckSuffix(Shared.Db, options.DbSuffix, ctx.SharedDb, fromEntityDbSuffix);
                ctx.SharedTb = CheckSuffix(Shared.Tb, options.TbSuffix, ctx.SharedTb, fromEntityTbSuffix);

                var parameters = (IDictionary<string, object>)ctx.Parameters;

                var columns = GetUpdateColumnList(options.UpdateColumns, options.UpdateIgnoreColumns);

                foreach (var propertyMetadata in GetPropertyMetadatas().Where(_ => columns.Contains(_.PropertyName)))
                {
                    parameters.Add(propertyMetadata.PropertyName, propertyMetadata.Property.GetValue(entity));
                }
            }

            var tableName = GetTableName() + ctx.SharedTb;

            //  TODO:分库 分表 字段禁止更新
            ctx.InsertAtStart($"UPDATE {WarpCoat(tableName)} SET {GetUpdateColumnSql(options.UpdateColumns, options.UpdateIgnoreColumns)}");

            ctx.Logger();

            return ctx;
        }

        public override SqlContext Delete(SqlOptions options, Expression<Func<T, bool>> predicate)
        {
            Debug.Assert(predicate != null);

            if (EntityProperties.HasFlag(EntityPropertyFlags.SoftDelete))
            {
                var entity = Activator.CreateInstance<T>();

                var property = GetPropertyMetadata(nameof(ISoftDeleteProperties.DeleteFlag));
                property.Property.SetValue(entity, true);

                options.UpdateColumns = new[] { property.PropertyName };

                return Update(options, entity, predicate);
            }
            else
            {
                //  TODO:这边有BUG，分片没设置
                var tableName = GetTableName() + options.TbSuffix;

                var ctx = new SqlContext(QueryType.Delete);

                //  DELETE FROM
                ctx.Append($"DELETE FROM {WarpCoat(tableName)}");
                //  WHERE
                AppendWhere(ctx, predicate);

                ctx.Logger();

                return ctx;
            }
        }

        private string CheckSuffix(Shared shared, string fromSpecifiedSuffix = null, string fromPredicateSuffix = null, string fromEntitySuffix = null)
        {
            var suffices = new[] { fromSpecifiedSuffix, fromPredicateSuffix, fromEntitySuffix }.Where(_ => _?.Length > 0).Distinct().ToArray();

            if (suffices.Length > 1)
            {
                throw new OutOfChoiceSharedException(shared, fromEntitySuffix, fromSpecifiedSuffix, fromPredicateSuffix);
            }

            var suffix = suffices.FirstOrDefault();

            if (suffix != null)
            {
                if (shared == Shared.Db)
                {
                    if (_entityReflection.SharedDb == null)
                    {
                        throw new NotSharedException<T>(Shared.Db, suffix);
                    }
                    else if (_entityReflection.SharedDb.Attribute.DisableDefaultValueShared)
                    {
                        _entityReflection.SharedDb.DisableDefaultValueShared(Shared.Db, suffix);
                    }
                }
                else
                {
                    if (_entityReflection.SharedTb == null)
                    {
                        throw new NotSharedException<T>(Shared.Tb, suffix);
                    }
                    else if (_entityReflection.SharedTb.Attribute.DisableDefaultValueShared)
                    {
                        _entityReflection.SharedTb.DisableDefaultValueShared(Shared.Tb, suffix);
                    }
                }
            }
            else
            {
                if (shared == Shared.Db && _entityReflection.SharedDb != null)
                {
                    throw new FailedGetSharedException<T>(shared);
                }

                if (shared == Shared.Tb && _entityReflection.SharedTb != null)
                {
                    throw new FailedGetSharedException<T>(shared);
                }
            }

            var a = suffix ?? "";

            return a;
        }
    }
}
