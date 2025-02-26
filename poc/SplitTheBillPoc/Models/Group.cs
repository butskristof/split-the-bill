namespace SplitTheBillPoc.Models;

internal sealed class Group
{
    public Guid Id { get; init; }
    public required string Name { get; set; }

    public ICollection<Member> Members { get; set; } = [];
    public ICollection<Expense> Expenses { get; set; } = [];

    public decimal TotalExpenses => Expenses.Sum(e => e.Amount);
}