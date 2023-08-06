namespace Luminous
{
    /// <summary>
    ///     提供反射功能
    /// </summary>
    public interface IRelection
    {
        IDictionary<string, PropertyEntry> GetProperties<T>();
        IDictionary<string, PropertyEntry> GetProperties(Type type);
    }
}
