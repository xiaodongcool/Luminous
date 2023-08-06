using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Luminous.Enumeration
{
    public static class EnumExtensions
    {
        public static string Display(this Enum enumValue) => enumValue.GetAttribute<DisplayAttribute>()?.Name ?? "";

        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<TAttribute>();
        }
    }
}
