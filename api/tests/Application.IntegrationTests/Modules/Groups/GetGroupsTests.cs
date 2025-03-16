using Shouldly;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.Tests.Shared.TestData.Builders;

namespace SplitTheBill.Application.IntegrationTests.Modules.Groups;

internal sealed class GetGroupsTests : ApplicationTestBase
{
    [Test]
    public async Task NoEntities_ReturnsEmptyList()
    {
        var result = await Application.SendAsync(new GetGroups.Request());

        result.IsError.ShouldBeFalse();
        var response = result.Value;
        response.ShouldNotBeNull();
        response.Groups.ShouldBeEmpty();
    }

    [Test]
    public async Task SingleEntity_ReturnsMappedEntity()
    {
        Guid groupId = new("5AA6E512-58CC-4224-A000-C9198CE5D6F8");
        const string groupName = "group name";

        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithName(groupName)
                .Build()
        );

        var result = await Application.SendAsync(new GetGroups.Request());

        result.IsError.ShouldBeFalse();
        var response = result.Value;
        response.ShouldNotBeNull();
        response.Groups.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                m => m.Id.ShouldBe(groupId),
                m => m.Name.ShouldBe(groupName)
            );
    }

    [Test]
    public async Task MultipleEntities_ReturnsMappedEntities()
    {
        Guid groupId1 = new("99E85C3C-8492-4C94-B267-00ACEFFEF8AF");
        const string groupName1 = "group name 1";
        Guid groupId2 = new("37A4B70F-CDB0-49C3-A344-16560BDD15A2");
        const string groupName2 = "group name 2";

        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId1)
                .WithName(groupName1)
                .Build(),
            new GroupBuilder()
                .WithId(groupId2)
                .WithName(groupName2)
                .Build()
        );

        var result = await Application.SendAsync(new GetGroups.Request());

        result.IsError.ShouldBeFalse();
        var response = result.Value;
        response.ShouldNotBeNull();
        response.Groups.Count.ShouldBe(2);
        response.Groups.ShouldContain(m =>
            m.Id == groupId1 && m.Name == groupName1);
        response.Groups.ShouldContain(m =>
            m.Id == groupId2 && m.Name == groupName2);
    }
}