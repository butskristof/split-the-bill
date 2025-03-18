using SplitTheBill.Application.Modules.Groups.Payments;

namespace SplitTheBill.Application.Tests.Shared.Builders;

public sealed class CreatePaymentRequestBuilder
{
    private Guid? _groupId = null;
    private Guid? _sendingMemberId = null;
    private Guid? _receivingMemberId = null;
    private decimal? _amount = null;

    public CreatePaymentRequestBuilder WithGroupId(Guid? groupId)
    {
        _groupId = groupId;
        return this;
    }

    public CreatePaymentRequestBuilder WithSendingMemberId(Guid? sendingMemberId)
    {
        _sendingMemberId = sendingMemberId;
        return this;
    }

    public CreatePaymentRequestBuilder WithReceivingMemberId(Guid? receivingMemberId)
    {
        _receivingMemberId = receivingMemberId;
        return this;
    }

    public CreatePaymentRequestBuilder WithAmount(decimal? amount)
    {
        _amount = amount;
        return this;
    }

    public CreatePayment.Request Build() => new()
    {
        GroupId = _groupId,
        SendingMemberId = _sendingMemberId,
        ReceivingMemberId = _receivingMemberId,
        Amount = _amount
    };

    public static implicit operator CreatePayment.Request(CreatePaymentRequestBuilder builder) => builder.Build();
}