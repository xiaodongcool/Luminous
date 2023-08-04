using System.Collections.Concurrent;
using System.Reflection;

namespace LangM.AspNetCore.DbInterface
{
    internal class AttributeCache
    {
        private static readonly ConcurrentDictionary<Type, Attribute[]> _cache = new();

        internal static T Get<T>(Type type) where T : Attribute
        {
            if (_cache.TryGetValue(type, out var attributes))
            {
                return (T)attributes.FirstOrDefault(_ => _ is T);
            }

            attributes = type.GetCustomAttributes().ToArray();
            _cache.TryAdd(type, attributes);
            return (T)attributes.FirstOrDefault(_ => _ is T);
        }
    }
}
