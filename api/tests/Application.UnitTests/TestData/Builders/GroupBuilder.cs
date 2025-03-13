using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.UnitTests.TestData.Builders;

internal sealed class GroupBuilder
{
    private Guid _id = Guid.Empty;
    private string _name = string.Empty;

    internal GroupBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    internal GroupBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    internal Group Build() => new()
    {
        Id = _id,
        Name = _name,
    };

    public static implicit operator Group(GroupBuilder builder) => builder.Build();
}