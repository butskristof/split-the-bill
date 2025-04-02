namespace SplitTheBill.Domain.Models.Groups;

public sealed class Payment
{
    public Guid Id { get; init; }
    public Guid GroupId { get; init; }

    public required Guid SendingMemberId { get; set; }
    public required Guid ReceivingMemberId { get; set; }
    
    public required decimal Amount { get; set; }
    
    public required DateTimeOffset Timestamp { get; set; }
}