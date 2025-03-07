namespace SplitTheBillPoc.Models;

internal sealed class Group
{
    public Guid Id { get; init; }
    public required string Name { get; set; }

    public ICollection<Member> Members { get; set; } = [];
    public ICollection<Expense> Expenses { get; set; } = [];
    public ICollection<Payment> Payments { get; set; } = [];
}