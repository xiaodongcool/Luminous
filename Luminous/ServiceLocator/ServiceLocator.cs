using Microsoft.Extensions.DependencyInjection;

namespace Luminous
{
    public static class ServiceLocator
    {
        private static IServiceProvider? _serviceProvider;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static T GetService<T>() where T : notnull
        {
            if (_serviceProvider == null)
            {
                throw new NotSupportedException("请在 Program 中使用 app.UseLuminousServiceLocator() 注册 ServiceLocator");
            }

            return _serviceProvider.GetRequiredService<T>();
        }

        public static T GetScopedService<T>() where T : notnull
        {
            var httpContext = new HttpContextAccessor().HttpContext;

            if (httpContext != null)
            {
                return httpContext.RequestServices.GetRequiredService<T>();
            }
            else
            {
                //  TODO: Global Log

                throw new Exception();
            }
        }
    }
}
