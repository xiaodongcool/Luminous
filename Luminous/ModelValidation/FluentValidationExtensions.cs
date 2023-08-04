using FluentValidation;
using FluentValidation.Validators;

namespace LangM.AspNetCore
{
    public static class FluentValidationExtensions
    {
        /// <summary>
        ///     手机号码
        /// </summary>
        public static IRuleBuilderOptions<T, string> Mobile<T>(this IRuleBuilder<T, string> ruleBuilder, EmailValidationMode mode = EmailValidationMode.AspNetCoreCompatible)
        {
            return ruleBuilder.SetValidator(new MobileValidator<T>());
        }
    }
}
