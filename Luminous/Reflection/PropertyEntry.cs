using System.Reflection;

namespace Luminous
{
    /// <summary>
    ///     属性信息
    /// </summary>
    public class PropertyEntry
    {
        public PropertyInfo Property { get; set; }
        public Type Type { get; set; }
        public LooseDataType LooseDataType { get; set; }
    }
}
