using Shouldly;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.UnitTests.TestData.Builders;

namespace SplitTheBill.Application.UnitTests.Modules.Groups;

internal sealed class GroupDtoTests
{
    [Test]
    public void GroupDto_MapsProperties()
    {
        var id = new Guid("62F29851-240D-4CAF-8543-7A1DA8EAE192");
        const string name = "group name";
        var group = new GroupBuilder()
            .WithId(id)
            .WithName(name)
            .Build();
        var dto = new GroupDto(group);

        dto.Id.ShouldBe(id);
        dto.Name.ShouldBe(name);
    }
}