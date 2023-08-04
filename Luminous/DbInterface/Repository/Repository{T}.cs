using Dapper;
using Microsoft.Extensions.Primitives;
using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Text;
using Microsoft.AspNetCore.Routing.Matching;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     仓储抽象类
    /// </summary>
    public abstract partial class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private static readonly ConcurrentDictionary<int, string[]> _cache = new();
        private readonly IShardEnsured _shardEnsured;
        private readonly ISql<T> _sql;
        private readonly IEntityReflection<T> _entityReflection;
        private string _tbSuffix;
        private string _dbSuffix;

        /// <summary>
        ///     数据库连接对象
        /// </summary>
        protected abstract IDbConnection DbConnection { get; }

        /// <summary>
        ///     是否分库
        /// </summary>
        public bool ShareDb => _entityReflection.SharedDb != null;

        /// <summary>
        ///     是否分表
        /// </summary>
        public bool ShareTb => _entityReflection.SharedTb != null;

        /// <summary>
        ///     分库后缀
        /// </summary>
        public virtual string DbSuffix
        {
            get => _dbSuffix;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(DbSuffix));
                }

                if (_entityReflection.SharedDb == null)
                {
                    throw new NotSharedException<T>(Shared.Db, value);
                }

                //  TODO:分表连接符号配置
                _dbSuffix = "_" + value;
            }
        }

        /// <summary>
        ///     分表后缀
        /// </summary>
        public virtual string TbSuffix
        {
            get => _tbSuffix ?? "";
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(TbSuffix));
                }

                if (_entityReflection.SharedTb == null)
                {
                    throw new NotSharedException<T>(Shared.Tb, value);
                }

                _tbSuffix = "_" + value;
            }
        }

        /// <summary>
        ///     construction
        /// </summary>
        protected Repository(IShardEnsured shardEnsured, ISql<T> sqlStatement, IEntityReflection<T> entityReflection)
        {
            _shardEnsured = shardEnsured;
            _sql = sqlStatement;
            _entityReflection = entityReflection;
        }

        /// <summary>
        ///     获取最终的分表后缀名
        /// </summary>
        public virtual string GetTbSuffixFinal(T entity = null)
        {
            var suffix = "";

            if (NotEmpty(TbSuffix))
            {
                //  手动指定分表后缀
                suffix = TbSuffix;
            }
            else if (entity != null)
            {
                //  通过实体获取分表后缀
                suffix = _entityReflection.SharedTb.GetSharedSuffix(entity);
            }

            if (NotEmpty(suffix))
            {
                //  数据库
                var dbName = GetDbName(false) + GetDbSuffixFinal(entity);

                //  确保表已经初始化
                //  TODO:看下这个方法还有哪里调，有问题的
                _shardEnsured.EnsureTb(dbName, GetTbName(false), suffix, true).GetAwaiter().GetResult();
            }
            else if (ShareTb)
            {
                throw new ForegoneException(WebApiStatusCode.InternalServerError, $"{typeof(T).FullName}是分表的，请指定分表后缀");
            }

            return suffix;
        }

        /// <summary>
        ///     获取最终的分库后缀名
        /// </summary>
        public virtual string GetDbSuffixFinal(T entity = null)
        {
            var suffix = "";

            if (NotEmpty(DbSuffix))
            {
                //  手动指定分库后缀
                suffix = DbSuffix;
            }
            else if (entity != null)
            {
                //  通过实体获取分库后缀
                suffix = _entityReflection.SharedDb.GetSharedSuffix(entity);
            }

            if (NotEmpty(suffix))
            {
                //  确保数据库已经初始化
                //  TODO:看下这个方法还有哪里调，有问题的
                _shardEnsured.EnsureDb(GetDbName(false), suffix, true).GetAwaiter().GetResult();
            }
            else if (ShareDb)
            {
                throw new ForegoneException(WebApiStatusCode.InternalServerError, $"{typeof(T).FullName}是分库的，请指定分库后缀");
            }

            return suffix;
        }

        public virtual async Task EnsureShared(SqlContext ctx)
        {
            Debug.Assert(ctx != null);
            Debug.Assert(ctx.EnsureStatus == SharedEnsureStatus.Sharding);
            Debug.Assert(!string.IsNullOrEmpty(ctx.SharedDb + ctx.SharedTb));

            if (!string.IsNullOrEmpty(ctx.SharedDb))
            {
                ctx.EnsureStatus = await _shardEnsured.EnsureDb(GetDbName(false), ctx.SharedDb, ctx.QueryType == QueryType.Insert);
            }

            if (!string.IsNullOrEmpty(ctx.SharedTb))
            {
                var dbName = GetDbName(false) + ctx.SharedDb;
                ctx.EnsureStatus = await _shardEnsured.EnsureTb(dbName, GetTbName(false), ctx.SharedTb, ctx.QueryType == QueryType.Insert);
            }

            Debug.Assert((int)ctx.EnsureStatus > 1);
        }

        /// <summary>
        ///     获取打在实体上的标签
        /// </summary>
        protected virtual TAttribute GetAttribute<TAttribute>() where TAttribute : Attribute
        {
            return AttributeCache.Get<TAttribute>(typeof(T));
        }

        /// <summary>
        ///     获取数据库名
        /// </summary>
        public abstract string GetDbName(bool withSuffix = true);

        /// <summary>
        ///     获取表名
        /// </summary>
        public abstract string GetTbName(bool withSuffix = true);

        protected virtual SqlOptions BuildSqlOptions(Expression<Func<T, object>> selectColumn = default, Expression<Func<T, object>> updateColumn = default, Expression<Func<T, object>> order = default, params OrderType[] orderTypes)
        {
            //  TODO:单元测试进一步完善 1、所有分片方式测试 2、单元测试重构，不要每个方法都添加条件，实体类型可以简化 3、复杂的条件查询额外隔开 4、查询指定列、更新指定列、排序 测试

            //  TODO:代码优化
            //  TODO:租户方案 确定（租户是一种特殊的分库方式）

            //  TODO:UOW 重构
            //  TODO:研究一下Sqlsugar 的 雪花id 和 uow

            //  TODO:跨库、跨表、跨服务器事务测试
            //  TODO:仓储方法加入事务对象

            var options = new SqlOptions
            {
                TbSuffix = TbSuffix,
                DbSuffix = DbSuffix
            };

            if (selectColumn != null)
            {
                options.SelectColumns = ParseProperties(selectColumn);
            }

            if (order != null)
            {
                var orderProperties = ParseProperties(order);

                if (orderProperties.Length > 0)
                {
                    var orderColumns = new List<OrderColumn>();

                    var orderTypesLength = orderTypes?.Length ?? 0;

                    for (var index = 0; index < orderProperties.Length; index++)
                    {
                        var orderType = OrderType.ASC;

                        if (index < orderTypesLength)
                        {
                            orderType = orderTypes[index];
                        }

                        var property = orderProperties[index];
                        orderColumns.Add(new OrderColumn(property, orderType));
                    }

                    options.Orders = orderColumns.ToArray();
                }
            }

            if (updateColumn != null)
            {
                options.UpdateColumns = ParseProperties(updateColumn);
            }

            return options;
        }

        public string[] ParseProperties(Expression<Func<T, object>> expr)
        {
            var properties = new List<string>();

            var type = typeof(T);
            if (expr.Body.NodeType == ExpressionType.Lambda || expr.Body.NodeType == ExpressionType.Convert)
            {
                if (expr.Body is UnaryExpression { Operand: MemberExpression expression })
                {
                    var prop = GetProperty(expression, type);
                    properties.Add(prop);
                }
            }
            else
            {
                if (expr.Body is NewExpression newExpression)
                {
                    var cols = newExpression?.Arguments;
                    if (cols != null)
                    {
                        foreach (var expression in cols)
                        {
                            var prop = GetProperty(expression, type);
                            if (string.IsNullOrEmpty(prop))
                                continue;
                            properties.Add(prop);
                        }
                    }
                }
            }

            return properties.ToArray();
        }

        private string GetProperty(Expression expression, Type type)
        {
            var field = (MemberExpression)expression;

            var prop = type.GetProperty(field.Member.Name);

            if (prop == null)
            {
                //  TODO:异常处理
                throw new Exception("不存的字段");
            }

            return prop.Name;
        }
    }

    /// <summary>
    ///     分片状态
    /// </summary>
    public enum SharedEnsureStatus
    {
        /// <summary>
        ///     单表，无需分片
        /// </summary>
        SingleTable,
        /// <summary>
        ///     分片执行中
        /// </summary>
        Sharding,
        /// <summary>
        ///     分片已存在
        /// </summary>
        Exists,
        /// <summary>
        ///     分片已创建
        /// </summary>
        Create,
        /// <summary>
        ///     分片未创建
        /// </summary>
        NotCreate
    }
}
