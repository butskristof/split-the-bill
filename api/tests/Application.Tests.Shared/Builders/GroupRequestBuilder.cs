using SplitTheBill.Application.Modules.Groups;

namespace SplitTheBill.Application.Tests.Shared.Builders;

public sealed class GroupRequestBuilder
{
    private Guid _id = Guid.NewGuid();
    private string? _name = "group name";

    public GroupRequestBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public GroupRequestBuilder WithName(string? name)
    {
        _name = name;
        return this;
    }

    public CreateGroup.Request BuildCreateRequest() => new() { Name = _name };

    public static implicit operator CreateGroup.Request(GroupRequestBuilder builder) =>
        builder.BuildCreateRequest();

    public UpdateGroup.Request BuildUpdateRequest() => new() { Id = _id, Name = _name };

    public static implicit operator UpdateGroup.Request(GroupRequestBuilder builder) =>
        builder.BuildUpdateRequest();
}
