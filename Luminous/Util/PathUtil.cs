using System.Reflection;

namespace Luminous
{
    public static class PathUtil
    {
        /// <summary>
        ///     获取程序运行根目录
        /// </summary>
        public static string GetBinPath()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }
    }
}
