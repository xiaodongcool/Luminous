using System.Reflection;

namespace Luminous
{
    public static class Util
    {
        /// <summary>
        ///     获取程序运行根目录
        /// </summary>
        public static string GetBinPath()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }
        public static string ParseString(DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static bool NotEmpty<T>(IEnumerable<T> array)
        {
            return !Empty(array);
        }

        public static bool Empty<T>(IEnumerable<T> array)
        {
            if (array == null)
            {
                return true;
            }

            return !array.Any();
        }

        public static bool NotEmpty(string value)
        {
            return !Empty(value);
        }

        public static bool Empty(string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}
