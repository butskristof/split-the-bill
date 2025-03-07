namespace SplitTheBillPoc.Models;

internal sealed class Expense
{
    public required Guid Id { get; set; }
    public required Guid GroupId { get; set; }
    
    public required string Description { get; set; }
    public required decimal Amount { get; set; }
}