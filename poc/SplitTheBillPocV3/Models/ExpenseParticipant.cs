namespace SplitTheBillPocV3.Models;

internal sealed class ExpenseParticipant
{
    public required Guid ExpenseId { get; init; }
    public required Guid MemberId { get; init; }

    public double? PercentualSplitShare { get; set; }
    public decimal? ExactAmountSplitShare { get; set; }
}