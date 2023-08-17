using System.Reflection;

namespace Luminous.Reflection
{
    /// <summary>
    ///     类型容器
    /// </summary>
    public static class TypeContainer
    {
        /// <summary>
        ///     获取所有类型
        /// </summary>
        public static IList<Type> FindAll() => Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(Assembly.Load).Union(AppDomain.CurrentDomain.GetAssemblies()).Where(x => !x?.FullName?.StartsWith("NPOI.Core") == true).SelectMany(x => x.DefinedTypes).Select(_ => _.AsType()).ToList();

        /// <summary>
        ///     查询 <see cref="T"/> 的所有子类
        /// </summary>
        public static IList<Type> FindChildClass<T>() => FindChildClass(typeof(T));

        public static IList<Type> FindChildInterface<T>() => FindChildInterface(typeof(T));

        /// <summary>
        ///     查询 <see cref="parent"/> 的所有子类
        /// </summary>
        public static IList<Type> FindChildInterface(Type parent)
        {
            if (parent.IsGenericType)
            {
                var result = new List<Type>();

                foreach (var type in FindAll().Where(type => type != parent && type.IsInterface))
                {
                    if (IsChildClass(parent, type))
                    {
                        result.Add(type);
                    }
                }

                return result;
            }
            else
            {
                return FindAll().Where(type => type != parent && parent.IsAssignableFrom(type) && type.IsInterface).ToList();
            }
        }

        /// <summary>
        ///     查询 <see cref="parent"/> 的所有子类
        /// </summary>
        public static IList<Type> FindChildClass(Type parent)
        {
            if (parent.IsGenericType)
            {
                var result = new List<Type>();

                foreach (var type in FindAll().Where(type => type != parent && type.IsClass && !type.IsAbstract))
                {
                    if (IsChildClass(parent, type))
                    {
                        result.Add(type);
                    }
                }

                return result;
            }
            else
            {
                return FindAll().Where(type => type != parent && parent.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract).ToList();
            }
        }

        /// <summary>
        ///     判断 <see cref="child"/> 是否是 <see cref="parent"/> 的子类
        /// </summary>
        public static bool IsChildClass(Type parent, Type child)
        {
            if (parent == null || child == null)
            {
                return false;
            }

            if (parent.IsGenericType)
            {
                var parentName = parent.Namespace + parent.Name;
                var childName = child.Namespace + child.Name;

                if (parentName == childName)
                {
                    return true;
                }
                else
                {
                    return IsChildClass(parent, child.BaseType);
                }
            }
            else
            {
                return child != parent && parent.IsAssignableFrom(child) && child.IsClass && !child.IsAbstract;
            }
        }

        /// <summary>
        ///     查询所有标记 <see cref="T"/> 的类
        /// </summary>
        public static IList<Type> FindAllTag<T>() where T : Attribute
        {
            var result = new List<Type>();

            foreach (var type in FindAll().Where(_ => _.IsClass && !_.IsAbstract))
            {
                if (type.GetCustomAttributes<T>().Any())
                {
                    result.Add(type);
                }
            }

            return result;
        }

        /// <summary>
        ///     是否是数字类型
        /// </summary>
        public static bool IsNumber(Type type)
        {
            Type[] numberTypes =
            {
                typeof(int), typeof(short), typeof(byte), typeof(float), typeof(decimal),
                typeof(double),
                typeof(long)
            };

            return numberTypes.Contains(type);
        }
    }
}
