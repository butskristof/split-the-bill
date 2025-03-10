namespace SplitTheBillPocV4.Models;

internal sealed class GroupMember
{
    public required Guid GroupId { get; init; }
    public required Guid MemberId { get; init; }
}