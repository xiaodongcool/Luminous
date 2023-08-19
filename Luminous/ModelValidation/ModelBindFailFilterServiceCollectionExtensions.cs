using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Luminous
{
    public static class ModelBindFailFilterServiceCollectionExtensions
    {
        /// <summary>
        ///     添加全局模型验证失败处理
        /// </summary>
        public static void AddModelValidation(this IServiceCollection services)
        {
            services.AddControllers().AddFluentValidation(fluent =>
            {
                Debug.Assert(Global.Solution != null);

                foreach (var validator in Global.Solution.GetDerivedClass(typeof(AbstractValidator<>)))
                {
                    fluent.RegisterValidatorsFromAssemblyContaining(validator);
                }
            });
        }
    }
}
