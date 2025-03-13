using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.Modules.Groups;

internal sealed record GroupDto
{
    private readonly Group _group;

    public GroupDto(Group group)
    {
        _group = group;
    }
    
    public Guid Id => _group.Id;
    public string Name => _group.Name;
}