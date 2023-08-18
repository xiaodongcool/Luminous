using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;

namespace Luminous.Reflection
{
    /// <summary>
    ///     类型容器
    /// </summary>
    public static class TypeContainer
    {
        static TypeContainer()
        {
            var assemblies = GetAssemblies(false);
            LoadedTypes = assemblies.Where(x => !x?.FullName?.StartsWith("NPOI.Core") == true).SelectMany(x => x.DefinedTypes).Select(_ => _.AsType()).ToArray();
        }

        public static Type[] LoadedTypes { get; }
        public static Type[] LoadedTypesWithSystemAndMicrosoft { get; }

        private static IEnumerable<Assembly> GetAssemblies(bool withSystemAndMicrosoft)
        {
            var result = new HashSet<Assembly>();

            var assembly = Assembly.GetEntryAssembly();

            if (assembly != null)
            {
                foreach (var item in assembly.GetReferencedAssemblies())
                {
                    if (string.IsNullOrEmpty(item.Name) || item.Name.StartsWith("System.") || item.Name.StartsWith("Microsoft."))
                    {
                        continue;
                    }

                    result.Add(Assembly.Load(item));
                }
            }

            foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (string.IsNullOrEmpty(item.FullName) || item.FullName.StartsWith("System.") || item.FullName.StartsWith("Microsoft."))
                {
                    continue;
                }

                result.Add(item);
            }

            return result;
        }

        //public static IEnumerable<Assembly> LoadBinFolderAssemblies(Predicate<string> predicate)
        //{
        //    var files = Directory.GetFiles(PathUtil.GetBinPath(), "*.dll");

        //    var assemblies = new HashSet<Assembly>();

        //    foreach (string file in files)
        //    {
        //        try
        //        {
        //            var filename = Path.GetFileName(file);

        //            if (!string.IsNullOrEmpty(filename))
        //            {
        //                if (IsSolutionProjectPredicate != null)
        //                {
        //                    if (IsSolutionProjectPredicate(filename))
        //                    {
        //                        var assembly = Assembly.LoadFrom(file);
        //                        assemblies.Add(assembly);
        //                    }
        //                }
        //                else
        //                {

        //                }
        //            }

        //            if (!IsExIncludeFile(filename) && predicate(filename))
        //            {
        //                var assembly = Assembly.LoadFrom(file);
        //                assemblies.Add(assembly);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Error loading {file}: {ex.Message}");
        //        }
        //    }

        //    return assemblies;
        //}


        private static Predicate<string>? IsSolutionProjectPredicate { get; set; }

        private static List<string> ExIncludeAssemblyPrefixs { get; set; } = new List<string>() { "System.", "Microsoft." };

        //private static IEnumerable<Assembly> GetAssemblies()
        //{
        //    var assembly = Assembly.GetEntryAssembly();

        //    Debug.Assert(assembly != null);

        //    var assemblies = new HashSet<Assembly>();
        //    var context = AssemblyLoadContext.GetLoadContext(assembly);

        //    Debug.Assert(context != null);

        //    CollectAssembliesAndDependenciesRecursive(assembly, assemblies, context);

        //    return assemblies;
        //}

        //static void CollectAssembliesAndDependenciesRecursive(Assembly assembly, HashSet<Assembly> assemblies, AssemblyLoadContext context)
        //{
        //    if (assemblies.Contains(assembly))
        //    {
        //        return;
        //    }

        //    assemblies.Add(assembly);

        //    foreach (var assemblyName in assembly.GetReferencedAssemblies())
        //    {
        //        var loadedAssembly = context.LoadFromAssemblyName(assemblyName);
        //        CollectAssembliesAndDependenciesRecursive(loadedAssembly, assemblies, context);
        //    }
        //}

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

                foreach (var type in LoadedTypes.Where(type => type != parent && type.IsInterface))
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
                return LoadedTypes.Where(type => type != parent && parent.IsAssignableFrom(type) && type.IsInterface).ToList();
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

                foreach (var type in LoadedTypes.Where(type => type != parent && type.IsClass && !type.IsAbstract))
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
                return LoadedTypes.Where(type => type != parent && parent.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract).ToList();
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

            foreach (var type in LoadedTypes.Where(_ => _.IsClass && !_.IsAbstract))
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
