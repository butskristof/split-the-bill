using FluentValidation.TestHelper;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.Tests.Shared.Builders;

namespace SplitTheBill.Application.UnitTests.Modules.Groups;

internal sealed class CreatePaymentValidatorTests
{
    private readonly CreatePayment.Validator _sut = new();

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
    public void NonEmptyGroupId_Passes()
    {
        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(new Guid("96C5E8EB-EC8E-4B7E-8C62-A59394364341"))
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.GroupId);
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
            .WithSendingMemberId(new Guid("FE1AB195-93FD-48E9-951F-A8BDEE0BBF22"))
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.SendingMemberId);
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
            .WithReceivingMemberId(new Guid("5864ED01-8A3B-4938-97EB-921A5A7375D5"))
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.ReceivingMemberId);
    }

    [Test]
    public void SameSendingAndReceivingMemberId_Fails()
    {
        Guid id = new("DA266E0C-86EA-4E91-97C1-CD19A72CC7C8");
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
            .WithGroupId(new Guid("04AFDBA3-991F-444E-8367-59742AA499C7"))
            .WithSendingMemberId(new Guid("0A1E7EA8-08EE-4B37-A6E4-2017D7EC90F4"))
            .WithReceivingMemberId(new Guid("2412AFDB-EA82-4520-A820-0A31FE0962F6"))
            .WithAmount(100m)
            .Build();
        var result = _sut.TestValidate(request);
        result
            .ShouldNotHaveAnyValidationErrors();
    }
}