using FluentValidation;
using FluentValidation.AspNetCore;
using Luminous.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Luminous.ModelValidation
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
                foreach (var validator in TypeContainer.FindChildClass(typeof(AbstractValidator<>)))
                {
                    fluent.RegisterValidatorsFromAssemblyContaining(validator);
                }
            });
        }
    }
}
