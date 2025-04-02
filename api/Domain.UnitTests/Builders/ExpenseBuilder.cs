using SplitTheBill.Domain.Models.Groups;
using SplitTheBill.Domain.Models.Members;

namespace SplitTheBill.Domain.UnitTests.Builders;

public sealed class ExpenseBuilder
{
    private Guid _id = Guid.NewGuid();
    private Guid _groupId = Guid.NewGuid();
    private string _description = "expense description";
    private Guid _paidByMemberId = Guid.NewGuid();
    private DateTimeOffset _timestamp = DateTimeOffset.UtcNow;

    public ExpenseBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public ExpenseBuilder WithGroupId(Guid groupId)
    {
        _groupId = groupId;
        return this;
    }

    public ExpenseBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ExpenseBuilder WithPaidByMember(Member member)
    {
        _paidByMemberId = member.Id;
        return this;
    }

    public ExpenseBuilder WithPaidByMemberId(Guid member)
    {
        _paidByMemberId = member;
        return this;
    }
    
    public ExpenseBuilder WithTimestamp(DateTimeOffset timestamp)
    {
        _timestamp = timestamp;
        return this;
    }

    public Expense Build() => new()
    {
        Id = _id,
        GroupId = _groupId,
        Description = _description,
        PaidByMemberId = _paidByMemberId,
        Timestamp = _timestamp,
    };

    public static implicit operator Expense(ExpenseBuilder builder) => builder.Build();

    public static Expense Create() => new ExpenseBuilder().Build();
}