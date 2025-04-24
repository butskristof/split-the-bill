namespace SplitTheBill.Domain.Models.Groups;

public sealed class GroupMember
{
    public Guid GroupId { get; init; }
    public Guid MemberId { get; init; }
}
