namespace SplitTheBillPocV4.Models;

// testing notes:
// - paid by non-participant
// - percentual requires total == 100%
// - exact amount requires total == amount
// - rounding errors?
// - participants are required
internal sealed class Expense
{
    public required Guid Id { get; init; }
    public required Guid GroupId { get; init; }

    public required string Description { get; set; }
    public required decimal Amount { get; set; }

    public ExpenseSplitType SplitType { get; set; } = ExpenseSplitType.Evenly;
    public List<ExpenseParticipant> Participants { get; init; } = [];

    public required Guid PaidByMemberId { get; set; }

    public decimal GetExpenseAmountForMember(Guid memberId) =>
        Participants.Any(p => p.MemberId == memberId)
            ? SplitType switch
            {
                ExpenseSplitType.Evenly => Participants.Count > 0 ? Amount / Participants.Count : 0,
                ExpenseSplitType.Percentual => Amount * Participants
                    .Single(p => p.MemberId == memberId)
                    .PercentualShare! ?? throw new ArgumentNullException(nameof(ExpenseParticipant.PercentualShare)),
                ExpenseSplitType.ExactAmount => Participants
                    .Single(p => p.MemberId == memberId)
                    .ExactAmountShare! ?? throw new ArgumentNullException(nameof(ExpenseParticipant.ExactAmountShare)),
                _ => throw new ArgumentOutOfRangeException($"Invalid {nameof(ExpenseSplitType)}"),
            }
            : 0m;
}

internal enum ExpenseSplitType : short
{
    Evenly = 0,
    Percentual = 1,
    ExactAmount = 2
}