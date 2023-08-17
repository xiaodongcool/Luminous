using Luminous.Serializer;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.AspNetCore.Builder
{
    public static class LuminousEndpointRouteBuilderExtensions
    {
        public static IEndpointConventionBuilder MapGetResult<T>(this IEndpointRouteBuilder endpoints, string pattern, Func<HttpRequest, IServiceProvider, Task<IResult<T>>> getResultFunc)
        {
            return endpoints.MapGet(pattern, async (httpContext) =>
            {
                var result = await getResultFunc(httpContext.Request, httpContext.RequestServices);
                var resultJson = JsonConvert.SerializeObject(result, Global.JsonSerializerSettings);
                httpContext.Response.ContentType = "text/plain; charset=utf-8";
                await httpContext.Response.WriteAsync(resultJson);
            });
        }

        public static IEndpointConventionBuilder MapGetResult(this IEndpointRouteBuilder endpoints, string pattern, Func<HttpRequest, IServiceProvider, Task<string>> getResultFunc)
        {
            return endpoints.MapGet(pattern, async (httpContext) =>
            {
                var result = await getResultFunc(httpContext.Request, httpContext.RequestServices);
                httpContext.Response.ContentType = "text/plain; charset=utf-8";
                await httpContext.Response.WriteAsync(result);
            });
        }
    }
}