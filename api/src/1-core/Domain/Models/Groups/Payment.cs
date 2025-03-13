namespace SplitTheBill.Domain.Models.Groups;

public sealed class Payment
{
    public required Guid Id { get; init; }
    public required Guid GroupId { get; init; }

    public required Guid SendingMemberId { get; init; }
    public required Guid ReceivingMemberId { get; init; }
    
    public required decimal Amount { get; set; }
}