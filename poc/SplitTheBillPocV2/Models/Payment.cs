namespace SplitTheBillPocV2.Models;

internal sealed class Payment
{
    public required Guid Id { get; init; }
    public required Guid GroupId { get; init; }
    public required Guid MemberId { get; init; }
    
    public required decimal Amount { get; set; }
}