using FluentValidation.TestHelper;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.Modules.Groups.Payments;

namespace SplitTheBill.Application.UnitTests.Modules.Groups.Payments;

internal sealed class DeletePaymentValidatorTests
{
    private readonly DeletePayment.Validator _sut = new();

    [Test]
    public void EmptyGroupId_Fails()
    {
        var request = new DeletePayment.Request(Guid.Empty, Guid.NewGuid());
        var result = _sut.TestValidate(request);
        result.ShouldHaveValidationErrorFor(r => r.GroupId).WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void ValidGroupId_Passes()
    {
        var request = new DeletePayment.Request(Guid.NewGuid(), Guid.NewGuid());
        var result = _sut.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(r => r.GroupId);
    }

    [Test]
    public void EmptyPaymentId_Fails()
    {
        var request = new DeletePayment.Request(Guid.NewGuid(), Guid.Empty);
        var result = _sut.TestValidate(request);
        result.ShouldHaveValidationErrorFor(r => r.PaymentId).WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void ValidPaymentId_Passes()
    {
        var request = new DeletePayment.Request(Guid.NewGuid(), Guid.NewGuid());
        var result = _sut.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(r => r.PaymentId);
    }

    [Test]
    public void ValidRequest_Passes()
    {
        var request = new DeletePayment.Request(Guid.NewGuid(), Guid.NewGuid());
        var result = _sut.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
