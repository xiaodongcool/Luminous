namespace Luminous
{
    /// <summary>
    ///     HTTP调用
    /// </summary>
    public interface IHttpInvocation
    {
        /// <summary>
        ///     HTTP GET
        /// </summary>
        Task<TResponse> Get<TResponse>(string url, IDictionary<string, string>? header = null);

        /// <summary>
        ///     HTTP POST
        /// </summary>
        Task<TResponse> Post<TResponse>(string url, string body, IDictionary<string, string>? header = null, string? contentType = null);

        /// <summary>
        ///     使用具名的 HttpClient
        /// </summary>
        void Use(string httpClientName);
    }
}
