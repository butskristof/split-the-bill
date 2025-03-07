namespace SplitTheBillPoc.Models;

internal sealed class Payment
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public required decimal Amount { get; set; }

    public required Guid PaidByMemberId { get; set; }
    public required Guid PaidToMemberId { get; set; }
}