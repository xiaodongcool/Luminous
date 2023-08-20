using System.Reflection;

namespace Luminous
{
    public class DynamicHttpInvocation : IDynamicHttpInvocation
    {
        private readonly IHttpInvocation httpInvocation;

        public DynamicHttpInvocation(IHttpInvocation httpInvocation)
        {
            this.httpInvocation = httpInvocation;
        }

        public object Get(Type tResponseType, string url, IDictionary<string, string>? header = null)
        {
            var getMethod = httpInvocation.GetType().GetMethod("Get", BindingFlags.Instance | BindingFlags.Public);
            var miAsync = getMethod.MakeGenericMethod(new Type[] { tResponseType });
            return (miAsync.Invoke(httpInvocation, new object[] { url, header }));
        }

        public object Post(Type tResponseType, string url, string body, IDictionary<string, string>? header = null, string? contentType = null)
        {
            var getMethod = httpInvocation.GetType().GetMethod("Post", BindingFlags.Instance | BindingFlags.Public);
            var miAsync = getMethod.MakeGenericMethod(new Type[] { tResponseType });
            return (miAsync.Invoke(httpInvocation, new object[] { url, body, header, contentType }));
        }
    }
}
