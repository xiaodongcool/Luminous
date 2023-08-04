namespace LangM.AspNetCore
{
    /// <summary>
    ///     提供 http 服务
    /// </summary>
    public interface IHttpCall
    {
        /// <summary>
        ///     HTTP GET
        /// </summary>
        Task<string> Get(string url, IDictionary<string, string> header);
        /// <summary>
        ///     HTTP POST
        /// </summary>
        Task<string> Post(string url, string body, IDictionary<string, string> header, string contentType);
        /// <summary>
        ///     使用具名的 HttpClient
        /// </summary>
        void Use(string httpClientName);
    }
}
