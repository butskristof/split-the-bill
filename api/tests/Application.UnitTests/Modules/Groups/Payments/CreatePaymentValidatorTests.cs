using FluentValidation.TestHelper;
using Shouldly;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.Modules.Groups.Payments;
using SplitTheBill.Application.Tests.Shared.Builders;

namespace SplitTheBill.Application.UnitTests.Modules.Groups.Payments;

internal sealed class CreatePaymentValidatorTests
{
    private readonly CreatePayment.Validator _sut = new();

    [Test]
    public void NullGroupId_Fails()
    {
        var request = new PaymentRequestBuilder()
            .WithGroupId(null)
            .BuildCreateRequest();
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
            .BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.GroupId)
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void EmptyNullableGuid_OnlyReturnsOneErrorCode()
    {
        var request = new PaymentRequestBuilder()
            .WithGroupId(Guid.Empty)
            .BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.GroupId)
            .Count().ShouldBe(1);
    }

    [Test]
    public void NonEmptyGroupId_Passes()
    {
        var request = new PaymentRequestBuilder()
            .WithGroupId(Guid.NewGuid())
            .BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.GroupId);
    }
    
    [Test]
    public void NullSendingMemberId_Fails()
    {
        var request = new PaymentRequestBuilder()
            .WithSendingMemberId(null)
            .BuildCreateRequest();
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
            .BuildCreateRequest();
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
            .BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.SendingMemberId);
    }
    
    [Test]
    public void NullReceivingMemberId_Fails()
    {
        var request = new PaymentRequestBuilder()
            .WithReceivingMemberId(null)
            .BuildCreateRequest();
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
            .BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.ReceivingMemberId)
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void NonEmptyReceivingMemberId_Passes()
    {
        var request = new PaymentRequestBuilder()
            .WithReceivingMemberId(Guid.NewGuid())
            .BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.ReceivingMemberId);
    }

    [Test]
    public void SameSendingAndReceivingMemberId_Fails()
    {
        var id = Guid.NewGuid();
        var request = new PaymentRequestBuilder()
            .WithSendingMemberId(id)
            .WithReceivingMemberId(id)
            .BuildCreateRequest();

        var result = _sut.TestValidate(request);
        result
            .ShouldHaveValidationErrorFor(r => r.ReceivingMemberId)
            .WithErrorMessage(ErrorCodes.NotUnique);
    }
    
    [Test]
    public void NullAmount_Fails()
    {
        var request = new PaymentRequestBuilder()
            .WithAmount(null)
            .BuildCreateRequest();
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
            .BuildCreateRequest();
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
            .BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.Amount);
    }

    [Test]
    public void ValidRequest_Passes()
    {
        var request = new PaymentRequestBuilder()
            .WithGroupId(Guid.NewGuid())
            .WithSendingMemberId(Guid.NewGuid())
            .WithReceivingMemberId(Guid.NewGuid())
            .WithAmount(100m)
            .BuildCreateRequest();
        var result = _sut.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }
}