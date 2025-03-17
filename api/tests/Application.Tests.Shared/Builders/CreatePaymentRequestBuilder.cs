using SplitTheBill.Application.Modules.Groups;

namespace SplitTheBill.Application.Tests.Shared.Builders;

public sealed class CreatePaymentRequestBuilder
{
    private Guid _groupId = Guid.NewGuid();
    private Guid _sendingMemberId = Guid.NewGuid();
    private Guid _receivingMemberId = Guid.NewGuid();
    private decimal _amount = 0.0m;

    public CreatePaymentRequestBuilder WithGroupId(Guid groupId)
    {
        _groupId = groupId;
        return this;
    }

    public CreatePaymentRequestBuilder WithSendingMemberId(Guid sendingMemberId)
    {
        _sendingMemberId = sendingMemberId;
        return this;
    }

    public CreatePaymentRequestBuilder WithReceivingMemberId(Guid receivingMemberId)
    {
        _receivingMemberId = receivingMemberId;
        return this;
    }

    public CreatePaymentRequestBuilder WithAmount(decimal amount)
    {
        _amount = amount;
        return this;
    }

    public CreatePayment.Request Build() => new(_groupId, _sendingMemberId, _receivingMemberId, _amount);
    public static implicit operator CreatePayment.Request(CreatePaymentRequestBuilder builder) => builder.Build();
}