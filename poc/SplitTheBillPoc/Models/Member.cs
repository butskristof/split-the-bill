namespace SplitTheBillPoc.Models;

public sealed class Member
{
    public Guid Id { get; init; }
    public required string Name { get; set; }
}