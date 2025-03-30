using FluentValidation.TestHelper;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.Modules.Groups.Payments;
using SplitTheBill.Application.Tests.Shared.Builders;

namespace SplitTheBill.Application.UnitTests.Modules.Groups.Payments;

internal sealed class UpdatePaymentValidatorTests
{
    private readonly UpdatePayment.Validator _sut = new();

    #region GroupId

    [Test]
    public void NullOrEmptyGroupId_Fails()
    {
        var request = new PaymentRequestBuilder()
            .WithGroupId(null)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.GroupId)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void EmptyGroupId_Fails()
    {
        var request = new PaymentRequestBuilder()
            .WithGroupId(Guid.Empty)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.GroupId)
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void NonEmptyGroupId_Passes()
    {
        var request = new PaymentRequestBuilder()
            .WithGroupId(Guid.NewGuid())
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.GroupId);
    }

    #endregion

    #region PaymentId

    [Test]
    public void NullPaymentId_Fails()
    {
        var request = new PaymentRequestBuilder()
            .WithPaymentId(null)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.PaymentId)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void EmptyPaymentId_Fails()
    {
        var request = new PaymentRequestBuilder()
            .WithPaymentId(Guid.Empty)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.PaymentId)
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void NonEmptyPaymentId_Passes()
    {
        var request = new PaymentRequestBuilder()
            .WithPaymentId(Guid.NewGuid())
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.PaymentId);
    }

    #endregion

    #region SendingMemberId

    [Test]
    public void NullSendingMemberId_Fails()
    {
        var request = new PaymentRequestBuilder()
            .WithSendingMemberId(null)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.SendingMemberId)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void EmptySendingMemberId_Fails()
    {
        var request = new PaymentRequestBuilder()
            .WithSendingMemberId(Guid.Empty)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.SendingMemberId)
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void NonEmptySendingMemberId_Passes()
    {
        var request = new PaymentRequestBuilder()
            .WithSendingMemberId(Guid.NewGuid())
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.SendingMemberId);
    }

    #endregion

    #region ReceivingMemberId

    [Test]
    public void NullReceivingMemberId_Fails()
    {
        var request = new PaymentRequestBuilder()
            .WithReceivingMemberId(null)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.ReceivingMemberId)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void EmptyReceivingMemberId_Fails()
    {
        var request = new PaymentRequestBuilder()
            .WithReceivingMemberId(Guid.Empty)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.ReceivingMemberId)
            .WithErrorMessage(ErrorCodes.Invalid);
    }
    
    [Test]
    public void SendingMemberIdEqualsReceivingMemberId_Fails()
    {
        var memberId = Guid.NewGuid();
        var request = new PaymentRequestBuilder()
            .WithSendingMemberId(memberId)
            .WithReceivingMemberId(memberId)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.ReceivingMemberId)
            .WithErrorMessage(ErrorCodes.NotUnique);
    }

    [Test]
    public void NonEmptyReceivingMemberId_Passes()
    {
        var request = new PaymentRequestBuilder()
            .WithReceivingMemberId(Guid.NewGuid())
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.ReceivingMemberId);
    }

    #endregion

    #region Amount

    [Test]
    public void NullAmount_Fails()
    {
        var request = new PaymentRequestBuilder()
            .WithAmount(null)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.Amount)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public void NegativeOrZeroAmount_Fails(decimal amount)
    {
        var request = new PaymentRequestBuilder()
            .WithAmount(amount)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.Amount)
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void PositiveAmount_Passes()
    {
        var request = new PaymentRequestBuilder()
            .WithAmount(1.0m)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.Amount);
    }

    #endregion

    [Test]
    public void ValidRequest_Passes()
    {
        var request = new PaymentRequestBuilder()
            .WithGroupId(Guid.NewGuid())
            .WithPaymentId(Guid.NewGuid())
            .WithSendingMemberId(Guid.NewGuid())
            .WithReceivingMemberId(Guid.NewGuid())
            .WithAmount(100)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }
}