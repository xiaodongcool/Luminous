using AspectCore.DependencyInjection;
using AspectCore.DynamicProxy;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Luminous
{
    [NonAspect]
    public class AspectCoreDependencyServiceProviderFactory : IServiceProviderFactory<IServiceContext>
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
