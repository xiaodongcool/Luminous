namespace Luminous
{
    /// <summary>
    ///     缓存key
    /// </summary>
    internal class CacheKey
    {
        private const string Prefix = "_sys_";

        public static string LockCreateDb(string newDbName) => $"{Prefix}:lock:{newDbName}";
        public static string DbEnsured => $"{Prefix}:db";

        public static string LockCreateTb(string dbName, string newTbName) => $"{Prefix}:lock:{dbName}:{newTbName}";
        public static string TbEnsured(string dbName) => $"{Prefix}:tb:{dbName}";
    }
}
