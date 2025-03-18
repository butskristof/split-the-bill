using SplitTheBill.Domain.Common;
using SplitTheBill.Domain.Models.Members;

namespace SplitTheBill.Domain.Models.Groups;

public sealed class Group : IAggregateRoot<Guid>
{
    public Guid Id { get; init; }
    public required string Name { get; set; }

    public List<Member> Members { get; init; } = [];
    public List<Expense> Expenses { get; init; } = [];
    public List<Payment> Payments { get; init; } = [];
}