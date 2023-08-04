using System.Diagnostics;
using System.Reflection;

namespace LangM.AspNetCore.DbInterface
{
    public interface IEntityReflection<T> where T : IEntity
    {
        EntityPropertyMetadata[] PropertyMetadatas { get; }
        EntityPropertyMetadata GetPropertyMetadata(string name);
        string TableName { get; }
        EntityPropertyFlags EntityPropertyFlags { get; }
        SharedMetadata SharedDb { get; }
        SharedMetadata SharedTb { get; }
    }

    public class SharedMetadata
    {
        private static readonly Type[] Number = { typeof(int), typeof(short), typeof(byte), typeof(long) };

        public SharedMetadata(PropertyInfo property, ShardAttribute shardAttribute)
        {
            if (shardAttribute != null && TypeContainer.IsChildClass(typeof(TimeSharedAttribute), shardAttribute.GetType()) && property.PropertyType != typeof(DateTime))
            {
                throw new ForegoneException(WebApiStatusCode.InternalServerError, $"{shardAttribute.GetType().FullName}只能用于时间类型");
            }

            if (shardAttribute != null && shardAttribute.GetType() == typeof(RemainderSharedAttribute) && !Number.Contains(property.PropertyType))
            {
                throw new ForegoneException(WebApiStatusCode.InternalServerError, $"{shardAttribute.GetType().FullName}只能用于数字类型");
            }

            Property = property;
            Attribute = shardAttribute;
        }
        public PropertyInfo Property { get; set; }
        public ShardAttribute Attribute { get; set; }
    }

    public static class SharedMetadataExtensions
    {
        /// <summary>
        ///     获取分片后缀
        /// </summary>
        public static string GetSharedSuffix<T>(this SharedMetadata sharedMetadata, T entity) where T : IEntity
        {
            if (sharedMetadata == null || entity == null)
            {
                return "";
            }

            var value = sharedMetadata.Property.GetValue(entity)?.ToString() ?? "";

            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            var suffix = sharedMetadata.Attribute.GetSharedSuffix(value);

            if (string.IsNullOrEmpty(suffix) == false)
            {
                return sharedMetadata.Attribute.Link + suffix;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        ///     获取分片后缀
        /// </summary>
        public static string GetSharedSuffix(this SharedMetadata sharedMetadata, object propertyValue)
        {
            if (sharedMetadata == null || propertyValue == null)
            {
                return "";
            }

            var value = propertyValue.ToString() ?? "";

            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            var suffix = sharedMetadata.Attribute.GetSharedSuffix(value);

            if (string.IsNullOrEmpty(suffix) == false)
            {
                return sharedMetadata.Attribute.Link + suffix;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        ///     禁止默认值分片
        /// </summary>
        public static void DisableDefaultValueShared(this SharedMetadata sharedMetadata, Shared shared, string suffix)
        {
            if (sharedMetadata == null)
            {
                return;
            }

            if (sharedMetadata.Attribute.DisableDefaultValueShared && TypeContainer.IsNumber(sharedMetadata.Property.PropertyType) && suffix == "_0")
            {
                throw new DisableDefaultValueSharedException(sharedMetadata.Property.DeclaringType, sharedMetadata.Property, shared);
            }
        }
    }
}
