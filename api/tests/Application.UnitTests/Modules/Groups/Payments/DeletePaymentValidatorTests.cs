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
        var request = new DeletePayment.Request(
            Guid.Empty,
            new Guid("FF5893B7-B213-4447-A4E3-34B6A2EA67CA")
        );
        var result = _sut.TestValidate(request);
        result
            .ShouldHaveValidationErrorFor(r => r.GroupId)
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void ValidGroupId_Passes()
    {
        var request = new DeletePayment.Request(
            new Guid("2606BE3F-01C9-421B-9E6F-0C46EA11A2E2"),
            new Guid("E8FD313E-31EA-4422-9FB1-7CB75D882E89")
        );
        var result = _sut.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(r => r.GroupId);
    }

    [Test]
    public void EmptyPaymentId_Fails()
    {
        var request = new DeletePayment.Request(
            new Guid("09912688-9C99-4308-8380-C9C86E01B030"),
            Guid.Empty
        );
        var result = _sut.TestValidate(request);
        result
            .ShouldHaveValidationErrorFor(r => r.PaymentId)
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void ValidPaymentId_Passes()
    {
        var request = new DeletePayment.Request(
            new Guid("FD23D959-C710-4EE1-B018-A65BE5D22E6C"),
            new Guid("67D086CF-71C8-4B8A-8816-FDFA81C99982")
        );
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