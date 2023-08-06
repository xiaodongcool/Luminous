using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ContactProviderServiceCollectionExtensions
    {
        public static void AddContactProvider(this IServiceCollection services)
        {
            services.TryAddSingleton<IResultFactory, DefaultResultFactory>();
        }
    }
}