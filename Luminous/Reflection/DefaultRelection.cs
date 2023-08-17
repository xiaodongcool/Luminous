using System.Collections.Concurrent;

namespace Luminous.Reflection
{
    /// <summary>
    ///     default implement for <see cref="IRelection"/>
    /// </summary>
    public class DefaultRelection : IRelection
    {
        private ConcurrentDictionary<Type, Dictionary<string, PropertyEntry>> _propertyCache = new();

        public IDictionary<string, PropertyEntry> GetProperties<T>() => GetProperties(typeof(T));

        public IDictionary<string, PropertyEntry> GetProperties(Type type)
        {
            var key = type;

            if (_propertyCache.TryGetValue(key, out var result))
            {
                return result;
            }

            result = new Dictionary<string, PropertyEntry>();

            foreach (var property in key.GetProperties())
            {
                result.Add(property.Name, new PropertyEntry
                {
                    Property = property,
                    Type = property.PropertyType,
                    LooseDataType = LooseDataTypeManage.Get(property.PropertyType)
                });
            }

            _propertyCache.TryAdd(key, result);
            return result;
        }

        public static class LooseDataTypeManage
        {
            private static readonly Type[] Integer = { typeof(int), typeof(short), typeof(byte), typeof(long), typeof(bool) };
            private static readonly Type[] Double = { typeof(double), typeof(float), typeof(decimal) };
            private static readonly Type[] String = { typeof(string), typeof(char) };
            private static readonly Type[] DateTime = { typeof(DateTime) };

            public static LooseDataType Get(Type type)
            {
                if (Integer.Contains(type))
                {
                    return LooseDataType.Integer;
                }
                else if (String.Contains(type))
                {
                    return LooseDataType.String;
                }
                else if (Double.Contains(type))
                {
                    return LooseDataType.Double;
                }
                else if (DateTime.Contains(type))
                {
                    return LooseDataType.DateTime;
                }
                else
                {
                    return LooseDataType.Complex;
                }
            }
        }
    }
}
