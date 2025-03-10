using SplitTheBillPocV4.Models;

namespace SplitTheBillPocV4.Tests.TestData.Builders;

internal class PaymentBuilder
{
    private Guid _id = Guid.Empty;
    private Guid _groupId = Guid.Empty;
    private Guid _sendingMemberId = Guid.Empty;
    private Guid _receivingMemberId = Guid.Empty;
    private decimal _amount = 0m;
    
    internal PaymentBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }
    
    internal PaymentBuilder WithGroupId(Guid groupId)
    {
        _groupId = groupId;
        return this;
    }
    
    internal PaymentBuilder WithSendingMemberId(Guid sendingMemberId)
    {
        _sendingMemberId = sendingMemberId;
        return this;
    }
    
    internal PaymentBuilder WithReceivingMemberId(Guid receivingMemberId)
    {
        _receivingMemberId = receivingMemberId;
        return this;
    }
    
    internal PaymentBuilder WithAmount(decimal amount)
    {
        _amount = amount;
        return this;
    }

    internal Payment Build() => new()
    {
        Id = _id,
        GroupId = _groupId,
        SendingMemberId = _sendingMemberId,
        ReceivingMemberId = _receivingMemberId,
        Amount = _amount
    };
    
    public static implicit operator Payment(PaymentBuilder builder) => builder.Build();
}