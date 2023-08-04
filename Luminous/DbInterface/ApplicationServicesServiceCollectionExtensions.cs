using LangM.AspNetCore.DbInterface;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationServicesServiceCollectionExtensions
    {
        /// <summary>
        ///     注册服务和仓储
        /// </summary>
        public static void AddApplication(this IServiceCollection services)
        {
            //  注册服务
            foreach (var type in TypeContainer.FindChildClass<ApplicationService>())
            {
                var interfaceType = type.GetTypeInfo().ImplementedInterfaces.FirstOrDefault(_ => _.Name == $"I{type.Name}");
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, type);
                }
            }

            //  注册仓储
            foreach (var type in TypeContainer.FindChildClass<IRepository>())
            {
                var interfaceType = type.GetTypeInfo().ImplementedInterfaces.FirstOrDefault(_ => _.Name == $"I{type.Name}");
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, type);
                }
            }
        }
    }
}
