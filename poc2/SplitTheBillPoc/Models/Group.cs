namespace SplitTheBillPoc.Models;

internal sealed class Group
{
    public required Guid Id { get; init; }
    public required string Name { get; set; }

    public List<Member> Members { get; set; } = [];
}