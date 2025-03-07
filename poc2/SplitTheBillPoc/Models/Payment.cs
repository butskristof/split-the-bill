namespace SplitTheBillPoc.Models;

internal sealed class Payment
{
    public required Guid Id { get; set; }
    public required Guid GroupId { get; set; }
    
    public required decimal Amount { get; set; }
}