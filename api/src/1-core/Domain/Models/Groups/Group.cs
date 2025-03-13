using SplitTheBill.Domain.Common;

namespace SplitTheBill.Domain.Models.Groups;

public sealed class Group : IAggregateRoot<Guid>
{
    public required Guid Id { get; init; }
    public required string Name { get; set; }
}