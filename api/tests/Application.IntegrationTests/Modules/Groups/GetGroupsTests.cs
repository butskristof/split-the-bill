using Shouldly;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.Tests.Shared.TestData;
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
        var groupId = Guid.NewGuid();
        const string groupName = "group name";

        await Application.AddAsync(
            new GroupBuilder().WithId(groupId).WithDefaultMember().WithName(groupName).Build()
        );

        var result = await Application.SendAsync(new GetGroups.Request());

        result.IsError.ShouldBeFalse();
        var response = result.Value;
        response.ShouldNotBeNull();
        response
            .Groups.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                m => m.Id.ShouldBe(groupId),
                m => m.Name.ShouldBe(groupName),
                m => m.MemberCount.ShouldBe(1)
            );
    }

    [Test]
    public async Task MultipleEntities_ReturnsMappedEntities()
    {
        var groupId1 = Guid.NewGuid();
        const string groupName1 = "group name 1";
        var groupId2 = Guid.NewGuid();
        const string groupName2 = "group name 2";

        await Application.AddAsync(
            new GroupBuilder().WithId(groupId1).WithDefaultMember().WithName(groupName1).Build(),
            new GroupBuilder().WithId(groupId2).WithDefaultMember().WithName(groupName2).Build()
        );

        var result = await Application.SendAsync(new GetGroups.Request());

        result.IsError.ShouldBeFalse();
        var response = result.Value;
        response.ShouldNotBeNull();
        response.Groups.Count.ShouldBe(2);
        response.Groups.ShouldContain(m =>
            m.Id == groupId1 && m.Name == groupName1 && m.MemberCount == 1
        );
        response.Groups.ShouldContain(m =>
            m.Id == groupId2 && m.Name == groupName2 && m.MemberCount == 1
        );
    }

    [Test]
    public async Task ReturnsOnlyGroupsWhereUserIdIsGroupMember()
    {
        var group1Id = Guid.NewGuid();
        var group2Id = Guid.NewGuid();

        await Application.AddAsync(
            new GroupBuilder().WithId(group1Id).Build(),
            new GroupBuilder().WithId(group2Id).WithMembers([TestMembers.Bob.Id]).Build()
        );
        Application.SetUserId(TestMembers.Bob.UserId);

        var result = await Application.SendAsync(new GetGroups.Request());
        var response = result.Value;

        response
            .Groups.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                g => g.Id.ShouldBe(group2Id),
                g => g.MemberCount.ShouldBe(1)
            );
    }

    [Test]
    public async Task NoGroupsForUserId_ReturnsEmptyList()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(new GroupBuilder().WithId(groupId).Build());

        Application.SetUserId(TestMembers.Alice.UserId);
        var result = await Application.SendAsync(new GetGroups.Request());
        result.IsError.ShouldBeFalse();
        result.Value.Groups.ShouldBeEmpty();
    }

    [Test]
    public async Task GroupWithMultipleMembers_ReturnsCorrectMemberCount()
    {
        var groupId = Guid.NewGuid();
        const string groupName = "group name";

        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithName(groupName)
                .WithMembers([TestMembers.Default.Id, TestMembers.Alice.Id, TestMembers.Bob.Id])
                .Build()
        );

        var result = await Application.SendAsync(new GetGroups.Request());

        result.IsError.ShouldBeFalse();
        var response = result.Value;
        response.ShouldNotBeNull();
        response
            .Groups.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                m => m.Id.ShouldBe(groupId),
                m => m.Name.ShouldBe(groupName),
                m => m.MemberCount.ShouldBe(3)
            );
    }
}
