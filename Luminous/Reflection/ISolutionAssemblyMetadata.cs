using System.Reflection;

namespace Luminous
{
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
}
