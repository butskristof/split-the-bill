namespace SplitTheBillPoc.Models;

internal sealed class Expense
{
    public Guid Id { get; set; }
    public required decimal Amount { get; set; }
    
    public required Guid GroupId { get; init; }
    public required Guid PaidByMemberId { get; set; }
}