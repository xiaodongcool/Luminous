using System.Net.Http.Headers;

namespace Luminous
{
    public class HttpInvocation : IHttpInvocation
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string _httpClientName = "default-invocation";

        public HttpInvocation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        ///     HTTP GET
        /// </summary>
        public async Task<TResponse> Get<TResponse>(string url, IDictionary<string, string>? header = null)
        {
            ArgumentGuard.CheckForNull(url, nameof(url));

            using var httpClient = _httpClientFactory.CreateClient(_httpClientName);

            SetHeader(header, httpClient);

            var response = await httpClient.GetAsync(url);

            EnsureResponse(response);

            var responseString = await response.Content.ReadAsStringAsync();

            return BuildResponse<TResponse>(responseString);
        }

        /// <summary>
        ///     HTTP POST
        /// </summary>
        public async Task<TResponse> Post<TResponse>(string url, string body, IDictionary<string, string>? header = null, string? contentType = null)
        {
            ArgumentGuard.CheckForNull(url, nameof(url));

            if (string.IsNullOrEmpty(contentType))
            {
                contentType = "application/json";
            }

            using var httpClient = _httpClientFactory.CreateClient(_httpClientName);
            using var httpContent = new StringContent(body ?? "");

            SetHeader(header, httpClient);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            try
            {
                var response = await httpClient.PostAsync(url, httpContent);

                EnsureResponse(response);

                var responseString = await response.Content.ReadAsStringAsync();

                return BuildResponse<TResponse>(responseString);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        /// <summary>
        ///     使用具名的 HttpClient
        /// </summary>
        public void Use(string name)
        {
            ArgumentGuard.CheckForNull(name, nameof(name));

            _httpClientName = name;
        }

        private static void SetHeader(IDictionary<string, string>? header, HttpClient httpClient)
        {
            if (header?.Count > 0)
            {
                foreach (var (key, value) in header)
                {
                    httpClient.DefaultRequestHeaders.Add(key, value);
                }
            }
        }

        private void EnsureResponse(HttpResponseMessage response)
        {
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new HttpRequestException("HTTP请求失败");
            }
        }

        private TResponse BuildResponse<TResponse>(string responseString)
        {
            var responseModel = JsonConvert.DeserializeObject<Result<TResponse>>(responseString);

            if (responseModel.Status != ResultStatus.Success)
            {
                throw new Exception("请求失败");
            }

            return responseModel.Payload;
        }
    }
}
