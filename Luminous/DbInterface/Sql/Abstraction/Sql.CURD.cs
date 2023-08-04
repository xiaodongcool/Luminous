using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;

namespace LangM.AspNetCore.DbInterface
{
    public abstract partial class Sql<T> : ISql<T> where T : IEntity
    {
        protected readonly IEntityReflection<T> _entityReflection;
        private readonly IDictionary<string, EntityPropertyMetadata> _propertyMetadata;

        protected EntityPropertyMetadata SoftDeletePropertyMetadata { get; }
        protected EntityPropertyFlags EntityProperties { get; }

        protected Sql(IEntityReflection<T> entityReflection)
        {
            _entityReflection = entityReflection;

            _propertyMetadata = _entityReflection.PropertyMetadatas.ToDictionary(_ => _.PropertyName, _ => _);

            if (_propertyMetadata.TryGetValue(nameof(IFullEntity.DeleteFlag), out var metadata))
            {
                SoftDeletePropertyMetadata = metadata;
            }

            EntityProperties = _entityReflection.EntityPropertyFlags;
        }

        protected EntityPropertyMetadata GetPropertyMetadata(string propertyName) => _propertyMetadata[propertyName];

        protected EntityPropertyMetadata[] GetPropertyMetadatas() => _entityReflection.PropertyMetadatas;

        protected bool ExistsProperty(string propertyName) => _propertyMetadata.ContainsKey(propertyName);

        protected string GetTableName() => _entityReflection.TableName;

        protected string GetSelectColumnSql(string[] columns)
        {
            string BuildColumnList(IEnumerable<EntityPropertyMetadata> properties)
            {
                return string.Join(",", properties.Select(_ => $"{WarpCoat(_.Column)} {WarpCoat(_.PropertyName)}"));
            }

            var properties = _propertyMetadata.Values;

            if (columns.Length == 0)
            {
                return BuildColumnList(properties);
            }
            else
            {
                return BuildColumnList(properties.Where(_ => columns.Contains(_.PropertyName)));
            }
        }

        protected string GetInsertColumnNameSql() => string.Join(",", _propertyMetadata.Values.Select(_ => $"{WarpCoat(_.Column)}"));

        protected string GetInsertColumnValueSql() => string.Join(",", _propertyMetadata.Values.Select(_ => "@" + _.PropertyName));

        protected IEnumerable<string> GetUpdateColumnList(string[] columns, string[] ignores)
        {
            Debug.Assert(columns.Length > 0 || ignores.Length > 0);

            if (columns.Length == 0)
            {
                return _propertyMetadata.Keys.Except(ignores);
            }
            else
            {
                return columns.Except(ignores);
            }
        }

        protected string GetUpdateColumnSql(string[] columns, string[] ignores)
        {
            Debug.Assert(columns.Length > 0 || ignores.Length > 0);

            string BuildColumnList(IEnumerable<string> properties)
            {
                return string.Join(",", _propertyMetadata.Values.Where(_ => properties.Contains(_.PropertyName)).Select(_ => $"{WarpCoat(_.Column)} = {"@" + _.PropertyName}"));
            }

            return BuildColumnList(GetUpdateColumnList(columns, ignores));
        }

        protected abstract string WarpCoat(string name);
        protected abstract string ParametersPrefix { get; }

        public abstract SqlContext Select(SqlOptions options, Expression<Func<T, bool>> predicate);
        public abstract SqlContext Counts(SqlOptions options, Expression<Func<T, bool>> predicate);
        public abstract SqlContext Insert(SqlOptions options, T entity);
        public abstract SqlContext Update(SqlOptions options, T entity, Expression<Func<T, bool>> predicate);
        public abstract SqlContext Delete(SqlOptions options, Expression<Func<T, bool>> predicate);
    }
}
