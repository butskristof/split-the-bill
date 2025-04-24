using FluentValidation.TestHelper;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.Modules.Groups.Expenses;

namespace SplitTheBill.Application.UnitTests.Modules.Groups.Expenses;

internal sealed class DeleteExpenseValidatorTests
{
    private readonly DeleteExpense.Validator _sut = new();

    [Test]
    public void EmptyGroupId_Fails()
    {
        var request = new DeleteExpense.Request(Guid.Empty, Guid.NewGuid());
        var result = _sut.TestValidate(request);
        result.ShouldHaveValidationErrorFor(r => r.GroupId).WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void ValidGroupId_Passes()
    {
        var request = new DeleteExpense.Request(Guid.NewGuid(), Guid.NewGuid());
        var result = _sut.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(r => r.GroupId);
    }

    [Test]
    public void EmptyExpenseId_Fails()
    {
        var request = new DeleteExpense.Request(Guid.NewGuid(), Guid.Empty);
        var result = _sut.TestValidate(request);
        result.ShouldHaveValidationErrorFor(r => r.ExpenseId).WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void ValidExpenseId_Passes()
    {
        var request = new DeleteExpense.Request(Guid.NewGuid(), Guid.NewGuid());
        var result = _sut.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(r => r.ExpenseId);
    }

    [Test]
    public void ValidRequest_Passes()
    {
        var request = new DeleteExpense.Request(Guid.NewGuid(), Guid.NewGuid());
        var result = _sut.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
