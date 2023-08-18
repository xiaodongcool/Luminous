using Microsoft.Extensions.DependencyInjection;
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

        Type[] GetChildTypes(Type type);
        Type[] GetChildClass(Type type);
        Type[] GetChildInterface(Type type);

        Type GetBaseType(Type type);
        Type[] GetBaseTypes(Type type);
        Type[] GetInterfaces(Type type);
        Type[] GetTypesWithAttribute(Type attributeType);
    }

    public class SolutionAssemblyMetadata : ISolutionAssemblyMetadata
    {
        public SolutionAssemblyMetadata(Predicate<string> assemblyNamePredicate)
        {

        }

        public Assembly[] GetAssemblies()
        {
            throw new NotImplementedException();
        }

        public Type GetBaseType(Type type)
        {
            throw new NotImplementedException();
        }

        public Type[] GetBaseTypes(Type type)
        {
            throw new NotImplementedException();
        }

        public Type[] GetChildClass(Type type)
        {
            throw new NotImplementedException();
        }

        public Type[] GetChildInterface(Type type)
        {
            throw new NotImplementedException();
        }

        public Type[] GetChildTypes(Type type)
        {
            throw new NotImplementedException();
        }

        public Type[] GetInterfaces(Type type)
        {
            throw new NotImplementedException();
        }

        public Type[] GetTypes()
        {
            throw new NotImplementedException();
        }

        public Type[] GetTypesWithAttribute(Type attributeType)
        {
            throw new NotImplementedException();
        }
    }

    public static class SolutionAssemblyMetadataServiceExtension
    {
        public static void AddLuminousAssemblyMetadata(this WebApplicationBuilder builder)
        {
            builder.Host.ConfigureServices((context, services) =>
            {
                var path = context.HostingEnvironment.ContentRootPath;
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
    }
}
