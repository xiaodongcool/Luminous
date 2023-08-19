using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Reflection;

namespace Luminous.Reflection
{
    /// <summary>
    ///     提供反射功能
    /// </summary>
    public interface IRelection
    {
        IDictionary<string, PropertyEntry> GetProperties<T>();
        IDictionary<string, PropertyEntry> GetProperties(Type type);
    }
    public interface ISolutionAssemblyMetadata
    {
        Assembly[] GetAssemblies();
        Type[] GetTypes();
        Type[] GetDerivedTypes(Type type);
        Type[] GetDerivedClass(Type type);
        Type[] GetDerivedInterface(Type type);
        Type? GetBaseType(Type type);
        Type[] GetBaseTypes(Type type);
        Type[] GetInterfaces(Type type);
        Type[] GetTypesWithAttribute(Type attributeType);
    }

    public class SolutionAssemblyMetadata : ISolutionAssemblyMetadata
    {
        private readonly Predicate<string>? _assemblyNamePredicate;

        private Assembly[]? _assemblies;
        private Type[]? _types;

        public SolutionAssemblyMetadata(Predicate<string>? assemblyNamePredicate = null)
        {
            _assemblyNamePredicate = assemblyNamePredicate;
        }

        private Assembly[] GetAssembliesInternal()
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

                    if (_assemblyNamePredicate == null || _assemblyNamePredicate(item.Name))
                    {
                        result.Add(Assembly.Load(item));
                    }
                }
            }

            foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (string.IsNullOrEmpty(item.FullName) || item.FullName.StartsWith("System.") || item.FullName.StartsWith("Microsoft."))
                {
                    continue;
                }

                if (_assemblyNamePredicate == null || _assemblyNamePredicate(item.FullName))
                {
                    result.Add(item);
                }
            }

            return result.ToArray();
        }

        public Assembly[] GetAssemblies()
        {
            if (_assemblies == null)
            {
                _assemblies = GetAssembliesInternal();
            }

            return _assemblies;
        }

        public Type? GetBaseType(Type type)
        {
            if (type == null)
            {
                return null;
            }

            var baseType = type.BaseType;

            if (baseType == null || baseType == typeof(object))
            {
                return null;
            }

            while (baseType.BaseType != null && baseType.BaseType != typeof(object))
            {
                baseType = baseType.BaseType;
            }

            return baseType;
        }


        public Type[] GetBaseTypes(Type type)
        {
            if (type == null || type.BaseType == null || type.BaseType == typeof(object))
            {
                return Array.Empty<Type>();
            }

            var baseTypes = new List<Type>();
            while (type.BaseType != null && type.BaseType != typeof(object))
            {
                baseTypes.Add(type.BaseType);
                type = type.BaseType;
            }

            return baseTypes.ToArray();
        }

        public Type[] GetDerivedClass(Type type)
        {
            if (type == null)
            {
                return Array.Empty<Type>();
            }

            var childClasses = new List<Type>();

            foreach (var t in GetTypes())
            {
                if (t.IsClass && (t.IsSubclassOf(type) || (type.IsInterface && t.GetInterfaces().Contains(type))))
                {
                    childClasses.Add(t);
                }
            }

            return childClasses.ToArray();
        }

        public Type[] GetDerivedInterface(Type type)
        {
            if (type == null || !type.IsInterface)
            {
                return Array.Empty<Type>();
            }

            var childInterfaces = new List<Type>();

            foreach (var t in GetTypes())
            {
                if (t.IsInterface && t.GetInterfaces().Contains(type))
                {
                    childInterfaces.Add(t);
                }
            }

            return childInterfaces.ToArray();
        }

        public Type[] GetDerivedTypes(Type type)
        {
            return GetDerivedClass(type).Union(GetDerivedInterface(type)).ToArray();
        }

        public Type[] GetInterfaces(Type type)
        {
            if (type == null)
            {
                return Array.Empty<Type>();
            }

            return type.GetInterfaces();
        }

        public Type[] GetTypes()
        {
            if (_types == null)
            {
                _types = GetAssemblies().SelectMany(x => x.DefinedTypes).Select(_ => _.AsType()).ToArray();
            }

            return _types;
        }

        public Type[] GetTypesWithAttribute(Type attributeType)
        {
            var result = new List<Type>();

            foreach (var t in GetTypes())
            {
                if (t.GetCustomAttributes(attributeType).Any())
                {
                    result.Add(t);
                }
            }

            return result.ToArray();
        }
    }

    public static class SolutionAssemblyMetadataServiceExtension
    {
        public static void AddLuminousAssemblyMetadata(this WebApplicationBuilder builder)
        {
            builder.Host.ConfigureServices((context, services) =>
            {
                //var path = context.HostingEnvironment.ContentRootPath;
                //var projectName = GetLastDirectoryName(path);
                //var solution = GetSolutionName(projectName);

                services.AddSingleton<ISolutionAssemblyMetadata>(serviceProvider => new SolutionAssemblyMetadata(null));
            });
        }

        public static void AddLuminousAssemblyMetadata(this WebApplicationBuilder builder, Predicate<string> assemblyNamePredicate)
        {
            if (assemblyNamePredicate == null)
            {
                throw new ArgumentNullException(nameof(assemblyNamePredicate));
            }

            builder.Host.ConfigureServices((context, services) =>
            {
                services.AddSingleton<ISolutionAssemblyMetadata>(serviceProvider => new SolutionAssemblyMetadata(assemblyNamePredicate));
            });
        }

        public static string GetLastDirectoryName(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
            {
                return string.Empty;
            }

            directoryPath = directoryPath.TrimEnd(Path.DirectorySeparatorChar);
            return Path.GetFileName(directoryPath);
        }

        public static string GetSolutionName(string projectName)
        {
            var array = projectName.Split('.').Where(x => !string.IsNullOrEmpty(x)).ToArray();

            if (array.Length == 1)
            {
                return array[0];
            }
            else
            {
                var guess = array.FirstOrDefault(x => !x.Contains("api", StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrEmpty(guess))
                {
                    return guess;
                }

                return array[0];
            }
        }
    }


    public static class LuminousServiceExtensions
    {
        public static void AddLuminous(this WebApplicationBuilder builder)
        {
            builder.AddLuminousAssemblyMetadata();
        }

        public static void AddLuminous(this WebApplicationBuilder builder, Predicate<string> assemblyNamePredicate)
        {
            builder.AddLuminousAssemblyMetadata(assemblyNamePredicate);
        }
    }
}
