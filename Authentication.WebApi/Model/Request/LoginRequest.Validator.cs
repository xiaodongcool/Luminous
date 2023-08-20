using FluentValidation;
using Luminous;

namespace Authentication.WebApi.Model
{
    public class LoginRequestValidator : ModelValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(request => request.UserAccount).NotEmpty().WithMessage("账号不能为空");
            RuleFor(request => request.Password).NotEmpty().WithMessage("密码不能为空");
        }
    }
}
