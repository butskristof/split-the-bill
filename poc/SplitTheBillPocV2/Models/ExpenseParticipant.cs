namespace SplitTheBillPocV2.Models;

internal sealed class ExpenseParticipant
{
    public required Guid ExpenseId { get; init; }
    public required Guid MemberId { get; init; }
}