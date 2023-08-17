namespace Luminous.Mapper
{
    /// <summary>
    ///     可以将被标记的模型映射为 <see cref="Type"/>
    /// </summary>
    public class MapToAttribute : Attribute
    {
        public Type Type { get; set; }

        public MapToAttribute(Type type)
        {
            Type = type;
        }
    }
}
