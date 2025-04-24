using FluentValidation;
using FluentValidation.TestHelper;
using Shouldly;
using SplitTheBill.Application.Common.Validation;

namespace SplitTheBill.Application.UnitTests.Common.Validation;

internal sealed record TestRequest(string? Value, string? OtherValue = "abc");

internal sealed class TestValidator : BaseValidator<TestRequest>
{
    public TestValidator()
    {
        RuleFor(r => r.Value)
            .NotNull()
            .WithMessage(ErrorCodes.Required)
            .NotEmpty()
            .WithMessage(ErrorCodes.Invalid);
        RuleFor(r => r.OtherValue).NotNull().WithMessage(ErrorCodes.Required);
    }
}

internal sealed class BaseValidatorTests
{
    private readonly TestValidator _sut = new();

    [Test]
    public void UsesRuleLevelCascadeModeStop()
    {
        var result = _sut.TestValidate(new TestRequest(null));
        // should only contain Required for the NotNull failure, not Invalid as well from the NotEmpty failure
        result
            .ShouldHaveValidationErrorFor(r => r.Value)
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(e => e.ErrorMessage.ShouldBe(ErrorCodes.Required));
    }

    [Test]
    public void UsesClassLevelCascadeModeContinue()
    {
        // verify whether each property is validated individually (with 'Stop' cascade mode tested above), if
        // configured wrong it would stop after the first failure
        var result = _sut.TestValidate(new TestRequest(null, null));
        result.Errors.Count.ShouldBe(2);

        result.Errors.Count.ShouldBe(2);
        result.ShouldHaveValidationErrorFor(r => r.Value);
        result.ShouldHaveValidationErrorFor(r => r.OtherValue);
    }
}
