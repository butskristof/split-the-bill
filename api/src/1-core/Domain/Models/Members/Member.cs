using SplitTheBill.Domain.Common;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Domain.Models.Members;

public sealed class Member : IAggregateRoot<Guid>
{
    public required Guid Id { get; init; }
    public required string Name { get; set; }
    
    public List<Group> Groups { get; init; } = [];
}