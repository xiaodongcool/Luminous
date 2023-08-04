namespace LangM.AspNetCore.DbInterface
{
    public interface IEntityReflection
    {
        EntityPropertyMetadata[] GetPropertyMetadata<T>();
        string GetTableName<T>();
        EntityPropertyFlags GetEntityProperties<T>();
    }
}
