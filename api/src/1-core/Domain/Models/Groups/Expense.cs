namespace SplitTheBill.Domain.Models.Groups;

public sealed class Expense
{
    public Guid Id { get; init; }
    public Guid GroupId { get; init; }

    public required string Description { get; set; }

    #region Amount & participants

    public decimal Amount
    {
        get;
        set
        {
            if (value <= 0) throw new ArgumentException("Amount should be a positive value", nameof(Amount));
            if (SplitType == ExpenseSplitType.ExactAmount && value != _participants.Sum(p => p.ExactShare))
                throw new ArgumentException("Amount should equal sum of exact split amounts", nameof(Amount));
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

        SplitType = ExpenseSplitType.Evenly;
        Amount = amount;
        _participants.Clear();
        _participants.AddRange(
            participants.Select(id => new ExpenseParticipant { MemberId = id })
        );
    }

    public void SetAmountAndParticipantsWithPercentualSplit(decimal amount, IReadOnlyDictionary<Guid, int> participants)
    {
        if (participants.Count == 0)
            throw new ArgumentException("List of participants cannot be empty", nameof(participants));
        if (participants.Any(p => p.Value < 0))
            throw new ArgumentException("All participant percentages should be at least 0", nameof(participants));
        if (participants.Sum(p => p.Value) != 100)
            throw new ArgumentException("Sum of participant percentages should add up to 100", nameof(participants));

        SplitType = ExpenseSplitType.Percentual;
        Amount = amount;
        _participants.Clear();
        _participants.AddRange(
            participants.Select(pair => new ExpenseParticipant { MemberId = pair.Key, PercentualShare = pair.Value })
        );
    }

    public void SetAmountAndParticipantsWithExactSplit(decimal amount, IReadOnlyDictionary<Guid, decimal> participants)
    {
        if (participants.Count == 0)
            throw new ArgumentException("List of participants cannot be empty", nameof(participants));
        if (participants.Any(p => p.Value < 0))
            throw new ArgumentException("All participant shares should be at least 0", nameof(participants));
        if (participants.Sum(p => p.Value) != amount)
            throw new ArgumentException("Sum of participant shares should add up to amount", nameof(participants));

        SplitType = ExpenseSplitType.ExactAmount;
        _participants.Clear();
        _participants.AddRange(
            participants.Select(pair => new ExpenseParticipant { MemberId = pair.Key, ExactShare = pair.Value })
        );
        // set amount last, otherwise validation on sum of exact shares will fail
        Amount = amount;
    }

    #endregion

    public required Guid PaidByMemberId { get; set; }
    
    public required DateTimeOffset Timestamp { get; set; }

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