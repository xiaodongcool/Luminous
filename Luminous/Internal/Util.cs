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

        public static int GetHashCodePermanent(this string @string)
        {
            unchecked
            {
                var hash1 = 5381;
                var hash2 = hash1;

                for (int i = 0; i < @string.Length && @string[i] != '\0'; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ @string[i];
                    if (i == @string.Length - 1 || @string[i + 1] == '\0')
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ @string[i + 1];
                }

                return Math.Abs(hash1 + (hash2 * 1566083941));
            }
        }
    }
}
