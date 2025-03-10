using SplitTheBillPocV4.Models;

namespace SplitTheBillPocV4.Tests.TestData.Builders;

internal class ExpenseBuilder
{
    private Guid _id = Guid.Empty;
    private Guid _groupId = Guid.Empty;
    private string _description = string.Empty;
    private decimal _amount = 0m;
    private ExpenseSplitType _splitType = ExpenseSplitType.Evenly;
    private List<ExpenseParticipant> _participants = [];

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

    internal ExpenseBuilder WithSplitType(ExpenseSplitType splitType)
    {
        _splitType = splitType;
        return this;
    }

    internal ExpenseBuilder WithParticipants(List<Member> participants)
    {
        var expenseParticipants = participants
            .Select(p => new ExpenseParticipantBuilder()
                .WithMemberId(p.Id)
                .Build())
            .ToList();
        return WithParticipants(expenseParticipants);
    }

    internal ExpenseBuilder AddParticipant(Member member)
    {
        return AddParticipant(new ExpenseParticipantBuilder()
            .WithMemberId(member.Id)
            .Build());
    }
    
    internal ExpenseBuilder WithParticipants(List<ExpenseParticipant> participants)
    {
        _participants = participants;
        return this;
    }

    internal ExpenseBuilder AddParticipant(ExpenseParticipant participant)
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
        SplitType = _splitType,
        Participants = _participants
    };

    public static implicit operator Expense(ExpenseBuilder builder) => builder.Build();
}