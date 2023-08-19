namespace Luminous.Reflection
{
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
