using SplitTheBill.Domain.Models.Groups;
using SplitTheBill.Domain.Models.Members;

namespace SplitTheBill.Application.Tests.Shared.TestData.Builders;

public sealed class ExpenseBuilder
{
    private Guid _id = Guid.NewGuid();
    private Guid _groupId = Guid.NewGuid();
    private string _description = "expense description";
    private Guid _paidByMemberId = Guid.NewGuid();
    private DateTimeOffset _timestamp = DateTimeOffset.UtcNow;
    private decimal _amount = 100;
    private ExpenseSplitType _splitType = ExpenseSplitType.Evenly;

    private HashSet<Guid> _evenSplitParticipants = [];
    private Dictionary<Guid, int> _percentualSplitParticipants = [];
    private Dictionary<Guid, decimal> _exactAmountSplitParticipants = [];

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

    public ExpenseBuilder WithEvenSplit(HashSet<Guid> participants)
    {
        _splitType = ExpenseSplitType.Evenly;
        _evenSplitParticipants = participants;
        return this;
    }

    public ExpenseBuilder WithPercentualSplit(Dictionary<Guid, int> participants)
    {
        _splitType = ExpenseSplitType.Percentual;
        _percentualSplitParticipants = participants;
        return this;
    }

    public ExpenseBuilder WithExactAmountSplit(Dictionary<Guid, decimal> participants)
    {
        _splitType = ExpenseSplitType.ExactAmount;
        _exactAmountSplitParticipants = participants;
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

    public Expense Build()
    {
        var expense = new Expense
        {
            Id = _id,
            GroupId = _groupId,
            Description = _description,
            PaidByMemberId = _paidByMemberId,
            Timestamp = _timestamp,
        };

        switch (_splitType)
        {
            case ExpenseSplitType.Evenly:
                expense.SetAmountAndParticipantsWithEvenSplit(_amount, _evenSplitParticipants);
                break;
            case ExpenseSplitType.Percentual:
                expense.SetAmountAndParticipantsWithPercentualSplit(_amount, _percentualSplitParticipants);
                break;
            case ExpenseSplitType.ExactAmount:
                expense.SetAmountAndParticipantsWithExactSplit(_amount, _exactAmountSplitParticipants);
                break;
            default:
                throw new InvalidOperationException("Unsupported split type");
        }

        return expense;
    }

    public static implicit operator Expense(ExpenseBuilder builder) => builder.Build();
}