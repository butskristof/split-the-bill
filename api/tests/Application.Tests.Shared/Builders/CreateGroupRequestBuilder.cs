using SplitTheBill.Application.Modules.Groups;

namespace SplitTheBill.Application.Tests.Shared.Builders;

public sealed class CreateGroupRequestBuilder
{
    private string? _name = null;

    public CreateGroupRequestBuilder WithName(string? name)
    {
        _name = name;
        return this;
    }

    public CreateGroup.Request Build() => new()
    {
        Name = _name,
    };
    
    public static implicit operator CreateGroup.Request(CreateGroupRequestBuilder builder) => builder.Build();
}