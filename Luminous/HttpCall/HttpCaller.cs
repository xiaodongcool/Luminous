using System.Net.Http.Headers;

namespace LangM.AspNetCore
{
    /// <summary>
    ///     提供 http 服务
    /// </summary>
    public class HttpCaller : IHttpCall
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string _httpClientName = "default-httpcaller";

        public HttpCaller(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        ///     HTTP GET
        /// </summary>
        public async Task<string> Get(string url, IDictionary<string, string> header)
        {
            ArgumentChecker.ThrowIfNull(url, nameof(url));

            using var httpClient = _httpClientFactory.CreateClient(_httpClientName);

            if (NotEmpty(header))
            {
                foreach (var (key, value) in header)
                {
                    httpClient.DefaultRequestHeaders.Add(key, value);
                }
            }

            var response = await httpClient.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }

        /// <summary>
        ///     HTTP POST
        /// </summary>
        public async Task<string> Post(string url, string body, IDictionary<string, string> header, string contentType)
        {
            ArgumentChecker.ThrowIfNull(url, nameof(url));
            ArgumentChecker.ThrowIfNull(contentType, nameof(contentType));

            using var httpClient = _httpClientFactory.CreateClient(_httpClientName);
            using var httpContent = new StringContent(body ?? "");

            if (NotEmpty(header))
            {
                foreach (var (key, value) in header)
                {
                    httpContent.Headers.Add(key, value);
                }
            }

            httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            var response = await httpClient.PostAsync(url, httpContent);
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }

        /// <summary>
        ///     使用具名的 HttpClient
        /// </summary>
        public void Use(string name)
        {
            ArgumentChecker.ThrowIfNull(name, nameof(name));

            _httpClientName = name;
        }
    }
}
