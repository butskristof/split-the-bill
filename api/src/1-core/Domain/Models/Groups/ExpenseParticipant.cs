namespace SplitTheBill.Domain.Models.Groups;

public sealed class ExpenseParticipant
{
    public Guid ExpenseId { get; init; }
    public Guid MemberId { get; init; }

    public int? PercentualShare { get; set; }
    public decimal? ExactShare { get; set; }
}
