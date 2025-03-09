namespace SplitTheBillPocV3.Models;

internal sealed class Group
{
    public required Guid Id { get; init; }
    public required string Name { get; set; }

    public List<Member> Members { get; init; } = [];
    public List<Expense> Expenses { get; init; } = [];
    public List<Payment> Payments { get; init; } = [];
}