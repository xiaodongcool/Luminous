//using System.Diagnostics;
//using System.Reflection;

//namespace LangM.AspNetCore.DbInterface
//{
//    /// <summary>
//    ///     通过给实体打特性定义分片方式
//    /// </summary>
//    public class AttributeSharedContainer : ISharedContainer
//    {
//        private static readonly Dictionary<Type, CacheEntry> Shareds = new();
//        static AttributeSharedContainer()
//        {
//            foreach (var entity in TypeContainer.FindChildClass<IEntity>())
//            {
//                var props = entity.GetProperties();

//                var entry = new CacheEntry();

//                foreach (var prop in props)
//                {
//                    var query = prop.GetCustomAttributes<ShardAttribute>().AsQueryable();
//                    var tbAttrs = query.Where(_ => _.Shared == Shared.Table).ToArray();
//                    var dbAttrs = query.Where(_ => _.Shared == Shared.Database).ToArray();

//                    if (tbAttrs.Length > 1) throw new ForegoneException(WebApiStatusCode.InternalServerError, $"不能为实体模型{entity.FullName}的属性{prop.Name}设置多个分表方式");
//                    if (dbAttrs.Length > 1) throw new ForegoneException(WebApiStatusCode.InternalServerError, $"不能为实体模型{entity.FullName}的属性{prop.Name}设置多个分库方式");

//                    if (tbAttrs.Length == 1)
//                    {
//                        if (entry.TbAttr != null) throw new ForegoneException(WebApiStatusCode.InternalServerError, $"不能为实体模型{entity.FullName}的多个属性设置分表");
//                        entry.TbAttr = tbAttrs[0];
//                        entry.TbProp = prop;
//                    }

//                    if (dbAttrs.Length == 1)
//                    {
//                        if (entry.DbAttr != null) throw new ForegoneException(WebApiStatusCode.InternalServerError, $"不能为实体模型{entity.FullName}的多个属性设置分库");
//                        entry.DbAttr = dbAttrs[0];
//                        entry.DbProp = prop;
//                    }
//                }

//                Validate(entry);

//                if (entry.DbProp != null || entry.TbProp != null)
//                {
//                    Shareds.Add(entity, entry);
//                }
//            }
//        }

//        public string GetTbSuffix<T>(T entity)
//        {
//            Debug.Assert(entity != null);

//            if (!Shareds.TryGetValue(typeof(T), out var entry))
//            {
//                return "";
//            }

//            if (entry.TbProp == null)
//            {
//                return "";
//            }

//            //  通过在实体上标记的特性获取分表后缀名
//            var suffix = entry.TbAttr.GetSharedSuffix(entry.TbProp.GetValue(entity)?.ToString());

//            if (NotEmpty(suffix))
//            {
//                return entry.TbAttr.Link + suffix;
//            }

//            return "";
//        }

//        public string GetDbSuffix<T>(T entity)
//        {
//            Debug.Assert(entity != null);

//            if (!Shareds.TryGetValue(typeof(T), out var entry))
//            {
//                return "";
//            }

//            if (entry.DbProp == null)
//            {
//                return "";
//            }

//            //  通过在实体上标记的特性获取分库后缀名
//            var suffix = entry.DbAttr.GetSharedSuffix(entry.DbProp.GetValue(entity)?.ToString());

//            if (NotEmpty(suffix))
//            {
//                return entry.DbAttr.Link + suffix;
//            }

//            return "";
//        }

//        public bool ShareDb<T>() => Shareds.TryGetValue(typeof(T), out var entry) && entry.DbAttr != null;

//        public bool ShareTb<T>() => Shareds.TryGetValue(typeof(T), out var entry) && entry.TbAttr != null;

//        private static void Validate(CacheEntry entry)
//        {
//            if (entry.DbAttr != null && TypeContainer.IsChildClass(typeof(TimeShardAttribute), entry.DbAttr.GetType()) && entry.DbProp.PropertyType != typeof(DateTime))
//            {
//                throw new ForegoneException(WebApiStatusCode.InternalServerError, $"{entry.DbAttr.GetType().FullName}只能用于时间类型");
//            }

//            if (entry.TbAttr != null && TypeContainer.IsChildClass(typeof(TimeShardAttribute), entry.TbAttr.GetType()) && entry.TbProp.PropertyType != typeof(DateTime))
//            {
//                throw new ForegoneException(WebApiStatusCode.InternalServerError, $"{entry.TbAttr.GetType().FullName}只能用于时间类型");
//            }

//            if (entry.DbAttr != null && entry.DbAttr.GetType() == typeof(RemainderShardAttribute) && !Number.Contains(entry.DbProp.PropertyType))
//            {
//                throw new ForegoneException(WebApiStatusCode.InternalServerError, $"{entry.DbAttr.GetType().FullName}只能用于数字类型");
//            }

//            if (entry.TbAttr != null && entry.TbAttr.GetType() == typeof(RemainderShardAttribute) && !Number.Contains(entry.TbProp.PropertyType))
//            {
//                throw new ForegoneException(WebApiStatusCode.InternalServerError, $"{entry.TbAttr.GetType().FullName}只能用于数字类型");
//            }
//        }

//        private static readonly Type[] Number = { typeof(int), typeof(short), typeof(byte), typeof(long) };

//        class CacheEntry
//        {
//            public ShardAttribute DbAttr { get; set; }
//            public ShardAttribute TbAttr { get; set; }
//            public PropertyInfo DbProp { get; set; }
//            public PropertyInfo TbProp { get; set; }
//        }
//    }
//}
