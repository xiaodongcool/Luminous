using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace LangM.AspNetCore.DbInterface
{
    public class EntityReflection<T> : IEntityReflection<T> where T : IEntity
    {
        public static readonly Type[] SupportPropertyTypes =
        {
            typeof(string), typeof(int), typeof(short), typeof(byte), typeof(float), typeof(decimal), typeof(double),
            typeof(long), typeof(bool), typeof(DateTime)
        };

        public EntityReflection()
        {
            var properties = new List<EntityPropertyMetadata>();

            var entity = typeof(T);

            foreach (var property in entity.GetProperties())
            {
                //  只支持简单类型、枚举；不支持可空类型，不支持复杂类型

                if (SupportPropertyTypes.Contains(property.PropertyType) || property.PropertyType.BaseType == typeof(Enum))
                {
                    var column = property.GetCustomAttribute<ColumnAttribute>()?.Name ?? property.Name;
                    properties.Add(new EntityPropertyMetadata(column, property.Name, property));
                }

                if (property.PropertyType.Name == typeof(Nullable<>).Name)
                {
                    throw new Exception("实体不支持可空类型字段");
                }

                //  Shared
                var query = property.GetCustomAttributes<ShardAttribute>().AsQueryable();
                var dbs = query.Where(_ => _.Shared == Shared.Db).ToArray();
                var tbs = query.Where(_ => _.Shared == Shared.Tb).ToArray();

                if (dbs.Length > 1 || dbs.Length == 1 && SharedDb != null) throw new ForegoneException(WebApiStatusCode.InternalServerError, $"不能为实体模型{entity.FullName}设置多个分表方式");
                if (tbs.Length > 1 || tbs.Length == 1 && SharedTb != null) throw new ForegoneException(WebApiStatusCode.InternalServerError, $"不能为实体模型{entity.FullName}设置多个分库方式");

                if (dbs.Length == 1)
                {
                    SharedDb = new SharedMetadata(property, dbs[0]);
                }

                if (tbs.Length == 1)
                {
                    SharedTb = new SharedMetadata(property, tbs[0]);
                }
            }

            TableName = entity.GetCustomAttribute<TableAttribute>()?.Name ?? entity.Name;

            PropertyMetadatas = properties.ToArray();

            if (typeof(ISoftDeleteProperties).IsAssignableFrom(entity)) EntityPropertyFlags |= EntityPropertyFlags.SoftDelete;
            if (typeof(ICreateUpdateTimeProperties).IsAssignableFrom(entity)) EntityPropertyFlags |= EntityPropertyFlags.CreateUpdateTime;
            if (typeof(IEnableProperties).IsAssignableFrom(entity)) EntityPropertyFlags |= EntityPropertyFlags.Enable;
            if (typeof(IUserIdProperties).IsAssignableFrom(entity)) EntityPropertyFlags |= EntityPropertyFlags.UserId;
            if (typeof(IUserNameProperties).IsAssignableFrom(entity)) EntityPropertyFlags |= EntityPropertyFlags.UserName;
        }

        public EntityPropertyMetadata[] PropertyMetadatas { get; }

        public EntityPropertyMetadata GetPropertyMetadata(string name) => PropertyMetadatas.First(_ => _.PropertyName == name);

        public string TableName { get; }

        public EntityPropertyFlags EntityPropertyFlags { get; } = EntityPropertyFlags.Id;

        public SharedMetadata SharedDb { get; }

        public SharedMetadata SharedTb { get; }
    }
}
