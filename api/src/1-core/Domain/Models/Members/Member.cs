using SplitTheBill.Domain.Common;

namespace SplitTheBill.Domain.Models.Members;

public sealed class Member : IAggregateRoot<Guid>
{
    public required Guid Id { get; init; }
}