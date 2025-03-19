using SplitTheBill.Domain.Models.Groups;
using SplitTheBill.Domain.Models.Members;

namespace SplitTheBill.Application.Tests.Shared.TestData.Builders;

public sealed class ExpenseBuilder
{
    private Guid _id = Guid.NewGuid();
    private Guid _groupId = Guid.NewGuid();
    private string _description = "expense description";
    private decimal _amount = 100;
    private ExpenseSplitType _splitType = ExpenseSplitType.Evenly;
    private List<ExpenseParticipant> _participants = [];
    private Guid _paidByMemberId = Guid.NewGuid();

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

    public ExpenseBuilder WithAmount(decimal amount)
    {
        _amount = amount;
        return this;
    }

    public ExpenseBuilder WithSplitType(ExpenseSplitType splitType)
    {
        _splitType = splitType;
        return this;
    }

    public ExpenseBuilder WithParticipants(List<ExpenseParticipant> participants)
    {
        _participants = participants;
        return this;
    }

    public ExpenseBuilder WithParticipants(List<Member> members)
    {
        _participants = members
            .Select(p => new ExpenseParticipantBuilder()
                .WithMemberId(p.Id)
                .Build()
            )
            .ToList();
        return this;
    }

    public ExpenseBuilder AddParticipant(ExpenseParticipant participant)
    {
        _participants.Add(participant);
        return this;
    }

    public ExpenseBuilder AddParticipant(Member member)
    {
        _participants.Add(new ExpenseParticipantBuilder()
            .WithMemberId(member.Id)
        );
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

    public Expense Build() => new()
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