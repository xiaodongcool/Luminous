using AspectCore.DependencyInjection;
using AspectCore.DynamicProxy;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Luminous.DynamicProxy
{
    [NonAspect]
    public class DependencyServiceProviderFactory : IServiceProviderFactory<IServiceContext>
    {
        public IServiceContext CreateBuilder(IServiceCollection services)
        {
            return services.ToServiceContext();
        }

        public IServiceProvider CreateServiceProvider(IServiceContext contextBuilder)
        {
            var serviceProvider = contextBuilder.Build();
            return serviceProvider;
        }
    }
}
