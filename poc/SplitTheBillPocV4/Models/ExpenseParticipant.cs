namespace SplitTheBillPocV4.Models;

internal sealed class ExpenseParticipant
{
    public required Guid ExpenseId { get; init; }
    public required Guid MemberId { get; init; }

    public int? PercentualShare { get; set; }
    public decimal? ExactAmountShare { get; set; }
}