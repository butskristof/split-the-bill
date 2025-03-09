using SplitTheBillPocV3.Models;

namespace SplitTheBillPocV3.Tests.TestData.Builders;

internal class PaymentBuilder
{
    private Guid _id = Guid.Empty;
    private Guid _groupId = Guid.Empty;
    private Guid _memberId = Guid.Empty;
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
    
    internal PaymentBuilder WithMemberId(Guid memberId)
    {
        _memberId = memberId;
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
        MemberId = _memberId,
        Amount = _amount
    };
    
    public static implicit operator Payment(PaymentBuilder builder) => builder.Build();
}