using System.Reflection;

namespace LangM.AspNetCore.DbInterface
{
    public class EntityPropertyMetadata
    {
        public EntityPropertyMetadata(string column, string propertyName, PropertyInfo property)
        {
            Column = column;
            PropertyName = propertyName;
            Property = property;
        }
        public string Column { get; set; }
        public string PropertyName { get; set; }
        public PropertyInfo Property { get; set; }
    }
}
