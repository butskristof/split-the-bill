namespace SplitTheBill.Domain.Models.Groups;

public sealed class Expense
{
    public required Guid Id { get; init; }
    public required Guid GroupId { get; init; }

    public required string Description { get; set; }
    public required decimal Amount { get; set; }

    public ExpenseSplitType SplitType { get; set; } = ExpenseSplitType.Evenly;
    public List<ExpenseParticipant> Participants { get; init; } = [];

    public required Guid PaidByMemberId { get; set; }
}