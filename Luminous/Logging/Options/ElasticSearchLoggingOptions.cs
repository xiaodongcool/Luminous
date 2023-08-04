namespace LangM.AspNetCore
{
    /// <summary>
    ///     elasticsearch 日志选项
    /// </summary>
    public class ElasticSearchLoggingOptions
    {
        /// <summary>
        ///     elasticsearch 服务地址
        /// </summary>
        public string[] Urls { get; set; }

        /// <summary>
        ///     索引格式
        /// </summary>
        public string IndexFormat { get; set; }
    }
}