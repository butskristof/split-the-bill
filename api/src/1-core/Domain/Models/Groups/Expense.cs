namespace SplitTheBill.Domain.Models.Groups;

public sealed class Expense
{
    // private default constructor for EF compatibility
    // private Expense() {}
    public Expense() {}
    
    public required Guid Id { get; init; }
    public required Guid GroupId { get; init; }

    public required string Description { get; set; }

    #region Participants

    public decimal Amount
    {
        get;
        private set
        {
            if (value <= 0) throw new ArgumentException("Amount should be a positive value", nameof(Amount));
            field = value;
        }
    }

    public ExpenseSplitType SplitType { get; private set; } = ExpenseSplitType.Evenly;
    private readonly List<ExpenseParticipant> _participants = [];
    public IReadOnlyList<ExpenseParticipant> Participants => _participants.AsReadOnly();

    public void SetAmountAndParticipantsWithEvenSplit(decimal amount, IReadOnlySet<Guid> participants)
    {
        if (participants.Count == 0)
            throw new ArgumentException("List of participants cannot be empty", nameof(participants));
        Amount = amount;
        _participants.Clear();
        _participants.AddRange(participants.Select(id => new ExpenseParticipant { MemberId = id }));
    }

    public void SetAmountAndParticipantsWithPercentualSplit(decimal amount, IReadOnlyDictionary<Guid, int> participants)
    {
        // throw participants.Count 0
        // throw sum != 100
    }

    public void SetAmountAndParticipantsWithExactSplit(decimal amount, IReadOnlyDictionary<Guid, decimal> participants)
    {
        // throw participants.Count 0
        // throw sum != amount
    }

    #endregion

    public required Guid PaidByMemberId { get; set; }

    public decimal GetExpenseAmountForMember(Guid memberId) =>
        Participants.Any(p => p.MemberId == memberId)
            ? SplitType switch
            {
                ExpenseSplitType.Evenly => Participants.Count > 0 ? Amount / Participants.Count : 0,
                ExpenseSplitType.Percentual =>
                    Amount *
                    (Participants
                         .Single(p => p.MemberId == memberId)
                         .PercentualShare! ??
                     throw new ArgumentNullException(nameof(ExpenseParticipant.PercentualShare))
                    ) / 100m,
                ExpenseSplitType.ExactAmount => Participants
                    .Single(p => p.MemberId == memberId)
                    .ExactShare! ?? throw new ArgumentNullException(nameof(ExpenseParticipant.ExactShare)),
                _ => throw new ArgumentOutOfRangeException($"Invalid {nameof(ExpenseSplitType)}"),
            }
            : 0m;
}