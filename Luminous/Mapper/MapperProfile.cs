using AutoMapper;
using LangM.AspNetCore.Enumeration;
using System.Reflection;

namespace LangM.AspNetCore
{
    /// <summary>
    ///     对所有标记 <see cref="MapFromAttribute"/> <see cref="MapToAttribute"/> 的模型注册 AutoMapper
    /// </summary>
    public class MapperProfile : Profile
    {
        private static readonly Dictionary<Type, IList<CacheEntry>> Cache = new();

        public MapperProfile()
        {
            BuildCache();

            foreach (var type in TypeContainer.FindAllTag<MapFromAttribute>())
            {
                var maps = type.GetCustomAttributes<MapFromAttribute>();

                foreach (var map in maps)
                {
                    CreateMap(map.Type, type).AfterMap((a, b) =>
                    {
                        if (Cache.TryGetValue(type, out var entries))
                        {
                            //  映射枚举类型
                            foreach (var entry in entries)
                            {
                                var enumValue = entry.EnumProp.GetValue(b) as Enum;
                                var display = enumValue.Display();
                                entry.StringProp.SetValue(b, display);
                            }
                        }
                    });
                }
            }

            foreach (var type in TypeContainer.FindAllTag<MapToAttribute>())
            {
                var maps = type.GetCustomAttributes<MapToAttribute>();

                foreach (var map in maps)
                {
                    CreateMap(type, map.Type);
                }
            }
        }

        private static void BuildCache()
        {
            var reflection = RequestServices.GetService<IRelection>();

            foreach (var type in TypeContainer.FindAllTag<MapFromAttribute>())
            {
                var maps = type.GetCustomAttributes<MapFromAttribute>();

                var typeProps = reflection.GetProperties(type);

                foreach (var (name, prop) in typeProps)
                {
                    if (name.EndsWith("Display", StringComparison.CurrentCulture) && prop.Property.PropertyType == typeof(string))
                    {
                        if (name.Length <= 7)
                        {
                            continue;
                        }

                        var enumPropName = name.Substring(0, name.Length - 7);

                        if (typeProps.ContainsKey(enumPropName))
                        {
                            var enumProp = typeProps[enumPropName].Property;

                            if (enumProp.PropertyType.BaseType == typeof(Enum))
                            {
                                if (Cache.TryGetValue(type, out var entries))
                                {
                                    entries.Add(new CacheEntry(prop.Property, enumProp));
                                }
                                else
                                {
                                    Cache.Add(type, new List<CacheEntry> { new CacheEntry(prop.Property, enumProp) });
                                }
                            }
                        }
                    }
                }
            }
        }

        public class CacheEntry
        {
            public CacheEntry(PropertyInfo stringProp, PropertyInfo enumProp)
            {
                StringProp = stringProp;
                EnumProp = enumProp;
            }
            public PropertyInfo StringProp { get; set; }
            public PropertyInfo EnumProp { get; set; }
        }
    }
}
