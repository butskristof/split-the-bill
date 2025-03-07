namespace SplitTheBillPoc.Models;

internal sealed class Member
{
    public required Guid Id { get; init; }
    public required string Name { get; set; }

    public List<Group> Groups { get; set; } = [];
}