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
        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(null)
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.GroupId)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void EmptyGroupId_Fails()
    {
        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(Guid.Empty)
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.GroupId)
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void EmptyNullableGuid_OnlyReturnsOneErrorCode()
    {
        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(Guid.Empty)
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.GroupId)
            .Count().ShouldBe(1);
    }

    [Test]
    public void NonEmptyGroupId_Passes()
    {
        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(Guid.NewGuid())
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.GroupId);
    }
    
    [Test]
    public void NullSendingMemberId_Fails()
    {
        var request = new CreatePaymentRequestBuilder()
            .WithSendingMemberId(null)
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.SendingMemberId)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void EmptySendingMemberId_Fails()
    {
        var request = new CreatePaymentRequestBuilder()
            .WithSendingMemberId(Guid.Empty)
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.SendingMemberId)
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void NonEmptySendingMemberId_Passes()
    {
        var request = new CreatePaymentRequestBuilder()
            .WithSendingMemberId(Guid.NewGuid())
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.SendingMemberId);
    }
    
    [Test]
    public void NullReceivingMemberId_Fails()
    {
        var request = new CreatePaymentRequestBuilder()
            .WithReceivingMemberId(null)
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.ReceivingMemberId)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void EmptyReceivingMemberId_Fails()
    {
        var request = new CreatePaymentRequestBuilder()
            .WithReceivingMemberId(Guid.Empty)
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.ReceivingMemberId)
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void NonEmptyReceivingMemberId_Passes()
    {
        var request = new CreatePaymentRequestBuilder()
            .WithReceivingMemberId(Guid.NewGuid())
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.ReceivingMemberId);
    }

    [Test]
    public void SameSendingAndReceivingMemberId_Fails()
    {
        var id = Guid.NewGuid();
        var request = new CreatePaymentRequestBuilder()
            .WithSendingMemberId(id)
            .WithReceivingMemberId(id)
            .Build();

        var result = _sut.TestValidate(request);
        result
            .ShouldHaveValidationErrorFor(r => r.ReceivingMemberId)
            .WithErrorMessage(ErrorCodes.NotUnique);
    }
    
    [Test]
    public void NullAmount_Fails()
    {
        var request = new CreatePaymentRequestBuilder()
            .WithAmount(null)
            .Build();
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
        var request = new CreatePaymentRequestBuilder()
            .WithAmount(amount)
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.Amount)
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void PositiveAmount_Passes()
    {
        var request = new CreatePaymentRequestBuilder()
            .WithAmount(1.0m)
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.Amount);
    }

    [Test]
    public void ValidRequest_Passes()
    {
        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(Guid.NewGuid())
            .WithSendingMemberId(Guid.NewGuid())
            .WithReceivingMemberId(Guid.NewGuid())
            .WithAmount(100m)
            .Build();
        var result = _sut.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }
}