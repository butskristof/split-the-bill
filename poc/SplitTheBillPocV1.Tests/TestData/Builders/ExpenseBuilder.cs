using SplitTheBillPocV1.Models;

namespace SplitTheBillPoc.Tests.TestData.Builders;

internal class ExpenseBuilder
{
    private Guid _id = Guid.Empty;
    private Guid _groupId = Guid.Empty;
    private string _description = string.Empty;
    private decimal _amount = 0m;
    
    internal ExpenseBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }
    
    internal ExpenseBuilder WithGroupId(Guid groupId)
    {
        _groupId = groupId;
        return this;
    }
    
    internal ExpenseBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }
    
    internal ExpenseBuilder WithAmount(decimal amount)
    {
        _amount = amount;
        return this;
    }

    internal Expense Build() => new()
    {
        Id = _id,
        GroupId = _groupId,
        Description = _description,
        Amount = _amount
    };
    
    public static implicit operator Expense(ExpenseBuilder builder) => builder.Build();
}