using FluentValidation;

namespace SplitTheBill.Application.Common.Validation;

internal abstract class BaseValidator<T> : AbstractValidator<T>
{
    protected BaseValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
    }
}
