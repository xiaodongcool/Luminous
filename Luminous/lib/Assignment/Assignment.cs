using Newtonsoft.Json.Linq;

namespace Luminous
{
    public abstract class Assignment : IAssignment
    {
        private readonly IHttpContextAccessorSuper _feature;

        protected HttpContext HttpContext { get; }

        protected Assignment(IHttpContextAccessorSuper feature)
        {
            _feature = feature;

            HttpContext = feature.HttpContext;
        }

        protected string GetHeader(string name)
        {
            return EmpToNull(HttpContext?.Request?.Headers?[name].ToString());
        }

        protected string GetQuery(string name)
        {
            return EmpToNull(HttpContext?.Request?.Query?[name].ToString());
        }

        protected string GetForm(string name)
        {
            try
            {
                return EmpToNull(HttpContext?.Request?.Form?[name].ToString());
            }
            catch
            {
                return null;
            }
        }

        protected async Task<string> GetBody(string name)
        {
            var body = await _feature.GetBody();

            if (string.IsNullOrEmpty(body))
            {
                return null;
            }

            var json = JsonConvert.DeserializeObject<JObject>(body);

            if (json == null)
            {
                return null;
            }

            var key = json.Children().Select(_ => _.Path).FirstOrDefault(_ => _?.Replace("_", "")?.ToLower() == name.ToLower());

            if (Empty(key))
            {
                return null;
            }

            var value = json[key]?.ToString();

            return EmpToNull(value);
        }

        protected string GetValue(string name)
        {
            var value = GetQuery(name) ?? GetHeader(name) ?? GetForm(name) ?? GetBody(name).Result;
            return value;
        }

        private string EmpToNull(string value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}
