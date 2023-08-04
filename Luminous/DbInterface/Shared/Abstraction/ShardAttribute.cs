namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     分片特性基类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ShardAttribute : Attribute
    {
        /// <summary>
        ///     schema 和 分片后缀之间的连接字符
        /// </summary>
        public string Link = "_";

        /// <summary>
        ///     分片方式
        /// </summary>
        public Shared Shared { get; set; }

        /// <summary>
        ///     禁止默认值分片
        /// </summary>
        public bool DisableDefaultValueShared { get; set; }

        protected ShardAttribute(Shared shared, bool disableDefaultValueShared = true)
        {
            Shared = shared;
            DisableDefaultValueShared = disableDefaultValueShared;
        }

        /// <summary>
        ///     获取分片的后缀
        /// </summary>
        public abstract string GetSharedSuffix(string value);
    }
}
