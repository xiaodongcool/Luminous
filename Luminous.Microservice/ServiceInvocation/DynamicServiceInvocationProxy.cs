using AspectCore.DynamicProxy;
using AspectCore.Extensions.Reflection;
using Nacos.V2;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace Luminous.Microservice
{
    /// <summary>
    ///     动态服务调用代理
    /// </summary>
    public class DynamicServiceInvocationProxy : IInterceptor
    {
        public bool AllowMultiple { get; set; }
        public bool Inherited { get; set; }
        public int Order { get; set; }

        public async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var dynamicHttpInvocation = ServiceLocator.GetService<IDynamicHttpInvocation>();

            var parameters = context.Parameters;

            var returnType = context.ServiceMethod.ReturnType.GetGenericArguments()[0];

            var urlAttribute = context.ServiceMethod.GetCustomAttribute<HttpMethodAttribute>();
            var serviceName = context.ServiceMethod?.DeclaringType?.GetCustomAttribute<ServiceNameAttribute>()?.Name;

            ArgumentGuard.CheckForNull(urlAttribute, nameof(urlAttribute));
            ArgumentGuard.CheckForNull(serviceName, nameof(serviceName));

            var baseUrl = await GetNaming(serviceName);
            var url = $"{baseUrl}" + urlAttribute.RequestUrl;

            if (urlAttribute.Method == HttpMethod.Get)
            {
                url += BuildQueryString(context);

                context.ReturnValue = dynamicHttpInvocation.Get(returnType, url, null);
            }
            else if (urlAttribute.Method == HttpMethod.Post)
            {
                var parameter = "";

                if (parameters.Length == 0)
                {
                    parameter = "";
                }
                else if (parameters.Length == 1 && parameters.Single().GetType().IsClass)
                {
                    parameter = JsonConvert.SerializeObject(parameters.Single());
                }
                else
                {
                    url += BuildQueryString(context);
                }

                context.ReturnValue = dynamicHttpInvocation.Post(returnType, url, parameter, null, null);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        ///     服务发现
        /// </summary>
        private async Task<string> GetNaming(string serviceName)
        {
            var instance = await ServiceLocator.GetService<INacosNamingService>().SelectOneHealthyInstance(serviceName);
            var host = $"{instance.Ip}:{instance.Port}";
            var baseUrl = instance.Metadata.TryGetValue("secure", out _) ? $"https://{host}" : $"http://{host}";
            return baseUrl;
        }

        /// <summary>
        ///     构建查询参数
        /// </summary>
        /// <returns></returns>
        private string BuildQueryString(AspectContext context)
        {
            var parameters = context.ServiceMethod.GetParameters();
            var parameterValues = context.Parameters;

            if (parameters.Length == 0)
            {
                return string.Empty;
            }

            var queryString = new StringBuilder("?");

            for (var i = 0; i < parameters.Length; i++)
            {
                queryString.Append(parameters[i].Name + '=' + parameterValues[i] + '&');
            }

            return queryString.ToString().TrimEnd('&');
        }
    }
}
