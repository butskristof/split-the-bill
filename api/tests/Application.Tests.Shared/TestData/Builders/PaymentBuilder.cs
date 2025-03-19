using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.Tests.Shared.TestData.Builders;

public sealed class PaymentBuilder
{
    private Guid _id = Guid.NewGuid();
    private Guid _groupId = Guid.NewGuid();
    private Guid _sendingMemberId = Guid.NewGuid();
    private Guid _receivingMemberId = Guid.NewGuid();
    private decimal _amount = 100m;

    public PaymentBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public PaymentBuilder WithGroupId(Guid groupId)
    {
        _groupId = groupId;
        return this;
    }

    public PaymentBuilder WithSendingMemberId(Guid sendingMemberId)
    {
        _sendingMemberId = sendingMemberId;
        return this;
    }

    public PaymentBuilder WithReceivingMemberId(Guid receivingMemberId)
    {
        _receivingMemberId = receivingMemberId;
        return this;
    }

    public PaymentBuilder WithAmount(decimal amount)
    {
        _amount = amount;
        return this;
    }

    public Payment Build() => new()
    {
        Id = _id,
        GroupId = _groupId,
        SendingMemberId = _sendingMemberId,
        ReceivingMemberId = _receivingMemberId,
        Amount = _amount
    };

    public static implicit operator Payment(PaymentBuilder builder) => builder.Build();
}