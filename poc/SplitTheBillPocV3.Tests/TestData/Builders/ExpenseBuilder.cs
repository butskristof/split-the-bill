using SplitTheBillPocV3.Models;

namespace SplitTheBillPocV3.Tests.TestData.Builders;

internal class ExpenseBuilder
{
    private Guid _id = Guid.Empty;
    private Guid _groupId = Guid.Empty;
    private string _description = string.Empty;
    private decimal _amount = 0m;
    private List<Member> _participants = [];

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

    internal ExpenseBuilder WithParticipants(List<Member> participants)
    {
        _participants = participants;
        return this;
    }

    internal ExpenseBuilder AddParticipant(Member participant)
    {
        _participants.Add(participant);
        return this;
    }

    internal Expense Build() => new()
    {
        Id = _id,
        GroupId = _groupId,
        Description = _description,
        Amount = _amount,
        Participants = _participants
    };

    public static implicit operator Expense(ExpenseBuilder builder) => builder.Build();
}