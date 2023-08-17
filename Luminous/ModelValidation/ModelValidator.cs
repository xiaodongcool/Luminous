using FluentValidation;

namespace Luminous.ModelValidation
{
    /// <summary>
    ///     模型验证基类
    /// </summary>
    public abstract class ModelValidator<T> : AbstractValidator<T> { }
}
