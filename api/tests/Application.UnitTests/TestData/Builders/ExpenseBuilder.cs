using SplitTheBill.Domain.Models.Groups;
using SplitTheBill.Domain.Models.Members;

namespace SplitTheBill.Application.UnitTests.TestData.Builders;

internal sealed class ExpenseBuilder
{
    private Guid _id = Guid.Empty;
    private Guid _groupId = Guid.Empty;
    private string _description = string.Empty;
    private decimal _amount = 0;
    private ExpenseSplitType _splitType = ExpenseSplitType.Evenly;
    private List<ExpenseParticipant> _participants = [];
    private Guid _paidByMemberId = Guid.Empty;

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

    internal ExpenseBuilder WithParticipants(List<ExpenseParticipant> participants)
    {
        _participants = participants;
        return this;
    }

    internal ExpenseBuilder WithParticipants(List<Member> members)
    {
        _participants = members
            .Select(p => new ExpenseParticipantBuilder()
                .WithMemberId(p.Id)
                .Build()
            )
            .ToList();
        return this;
    }

    internal ExpenseBuilder WithParticipant(ExpenseParticipant participant)
    {
        _participants.Add(participant);
        return this;
    }

    internal ExpenseBuilder WithParticipant(Member member)
    {
        _participants.Add(new ExpenseParticipantBuilder()
            .WithMemberId(member.Id)
        );
        return this;
    }

    internal ExpenseBuilder WithPaidByMember(Member member)
    {
        _paidByMemberId = member.Id;
        return this;
    }

    internal ExpenseBuilder WithPaidByMemberId(Guid member)
    {
        _paidByMemberId = member;
        return this;
    }

    internal Expense Build() => new()
    {
        Id = _id,
        GroupId = _groupId,
        Description = _description,
        Amount = _amount,
        SplitType = _splitType,
        Participants = _participants,
        PaidByMemberId = _paidByMemberId
    };

    public static implicit operator Expense(ExpenseBuilder builder) => builder.Build();
}