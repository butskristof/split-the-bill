using SplitTheBill.Application.Modules.Groups;

namespace SplitTheBill.Application.Tests.Shared.Builders;

public sealed class UpdateGroupRequestBuilder
{
    private Guid _id = Guid.Empty;
    private string? _name = null;

    public UpdateGroupRequestBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public UpdateGroupRequestBuilder WithName(string? name)
    {
        _name = name;
        return this;
    }

    public UpdateGroup.Request Build() => new()
    {
        Id = _id,
        Name = _name,
    };

    public static implicit operator UpdateGroup.Request(UpdateGroupRequestBuilder builder) => builder.Build();
}