using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace Luminous.ModelValidation.Validators
{
    /// <summary>
    ///     手机号码
    /// </summary>
    public class MobileValidator<T> : PropertyValidator<T, string>, IEmailValidator
    {
        public override string Name => "MobileValidator";

        public override bool IsValid(ValidationContext<T> context, string value)
        {
            var regex = new Regex(@"^1\d{10}$");

            if (value == null)
            {
                return true;
            }

            return regex.IsMatch(value);
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            return Localized(errorCode, Name);
        }
    }
}
