namespace SplitTheBill.Domain.Models.Groups;

public sealed class ExpenseParticipant
{
    public required Guid ExpenseId { get; init; }
    public required Guid MemberId { get; init; }

    public int? PercentualShare { get; set; }
    public decimal? ExactShare { get; set; }
}