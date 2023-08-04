namespace LangM.AspNetCore
{
    /// <summary>
    ///     可以将 <see cref="Type"/> 映射为被标记的模型
    /// </summary>
    public class MapFromAttribute : Attribute
    {
        public Type Type { get; set; }

        public MapFromAttribute(Type type)
        {
            Type = type;
        }
    }
}
