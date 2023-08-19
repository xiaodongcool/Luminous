using System.Reflection;

namespace Luminous
{
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
            var files = Directory.GetFiles(PathUtil.GetBinPath(), "*.dll");

            var assemblies = new HashSet<Assembly>();

            foreach (string file in files)
            {
                try
                {
                    var filename = Path.GetFileName(file);

                    if (_assemblyNamePredicate != null)
                    {
                        if (_assemblyNamePredicate(filename))
                        {
                            assemblies.Add(Assembly.LoadFrom(file));
                        }
                    }
                    else
                    {
                        if (!IsExIncludeFile(filename))
                        {
                            assemblies.Add(Assembly.LoadFrom(file));
                        }
                    }
                }
                catch (Exception)
                {
                    //  TODO: Global Log
                }
            }

            return assemblies.ToArray();
        }

        private static bool IsExIncludeFile(string filename)
        {
            var exIncludes = new[] { "System.", "Microsoft.", "Newtonsoft.", "NPOI.", "Swashbuckle.", "NewLife.", "SQLitePCLRaw.", "SixLabors.", "RazorEngine."
            , "RabbitMQ."
            , "Quartz."
            , "Oracle."
            , "Npgsql."
            , "MySqlConnector."
            , "MathNet."
            , "Mapster."
            , "Kdbndp."
            , "JWT."
            , "IdentityModel."
            , "ICSharpCode."
            , "Enums."
            , "dotnet-aspnet-codegenerator-design."
            , "DmProvider"
            , "BouncyCastle."
            , "Autofac."
            , "Aliyun."
            , "aliyun-net-sdk-core"
            };

            return string.IsNullOrEmpty(filename) || exIncludes.Any(x => filename.StartsWith(x));
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
}
