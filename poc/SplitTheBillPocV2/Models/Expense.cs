namespace SplitTheBillPocV2.Models;

internal sealed class Expense
{
    public required Guid Id { get; init; }
    public required Guid GroupId { get; init; }
    
    public required string Description { get; set; }
    public required decimal Amount { get; set; }

    public List<Member> Participants { get; init; } = [];
}