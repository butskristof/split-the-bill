namespace SplitTheBillPoc.Models;

internal sealed class Group
{
    public Guid Id { get; init; }
    public required string Name { get; set; }

    public IEnumerable<Member> Members { get; set; } = [];
    public IEnumerable<Expense> Expenses { get; set; } = [];
}