using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace LangM.AspNetCore.DbInterface
{
    public class EntityReflection : IEntityReflection
    {
        private static readonly Dictionary<Type, EntityReflectionEntry> Cache = new();

        private static readonly Type[] SupportPropertyTypes =
        {
            typeof(string), typeof(int), typeof(short), typeof(byte), typeof(float), typeof(decimal), typeof(double),
            typeof(long), typeof(bool), typeof(DateTime)
        };

        public EntityReflection()
        {
            foreach (var entity in TypeContainer.FindChildClass<IEntity>())
            {
                var properties = new List<EntityPropertyMetadata>();

                foreach (var property in entity.GetProperties())
                {
                    if (SupportPropertyTypes.Contains(property.PropertyType) || property.PropertyType.BaseType == typeof(Enum))
                    {
                        var column = property.GetCustomAttribute<ColumnAttribute>()?.Name ?? property.Name;
                        properties.Add(new EntityPropertyMetadata(column, property.Name, property));
                    }

                    //  只支持简单类型、枚举；不支持可空类型，不支持复杂类型
                }

                var table = entity.GetCustomAttribute<TableAttribute>()?.Name ?? entity.Name;

                if (Cache.ContainsKey(entity) == false)
                {
                    var flags = EntityPropertyFlags.Id;

                    if (typeof(ISoftDeleteProperties).IsAssignableFrom(entity)) flags |= EntityPropertyFlags.SoftDelete;
                    if (typeof(ICreateUpdateTimeProperties).IsAssignableFrom(entity)) flags |= EntityPropertyFlags.CreateUpdateTime;
                    if (typeof(IEnableProperties).IsAssignableFrom(entity)) flags |= EntityPropertyFlags.Enable;
                    if (typeof(IUserIdProperties).IsAssignableFrom(entity)) flags |= EntityPropertyFlags.UserId;
                    if (typeof(IUserNameProperties).IsAssignableFrom(entity)) flags |= EntityPropertyFlags.UserName;

                    Cache.Add(entity, new EntityReflectionEntry(table, properties.ToArray(), flags));
                }
            }
        }

        public EntityPropertyMetadata[] GetPropertyMetadata<T>() => Cache[typeof(T)].PropertyMetadata;

        public string GetTableName<T>() => Cache[typeof(T)].TableName;

        public EntityPropertyFlags GetEntityProperties<T>() => Cache[typeof(T)].EntityProperties;
    }
}
