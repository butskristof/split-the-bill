using ErrorOr;
using Shouldly;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.Tests.Shared.TestData;
using SplitTheBill.Application.Tests.Shared.TestData.Builders;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.IntegrationTests.Modules.Groups;

internal sealed class DeleteGroupTests : ApplicationTestBase
{
    [Test]
    public async Task InvalidRequest_ReturnsValidationError()
    {
        var request = new DeleteGroup.Request(Guid.Empty);
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result
            .ErrorsOrEmptyList.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.Validation),
                e => e.Code.ShouldBe(nameof(request.Id)),
                e => e.Description.ShouldBe(ErrorCodes.Invalid)
            );
    }

    [Test]
    public async Task GroupDoesNotExist_ReturnsNotFoundError()
    {
        await Application.AddAsync(
            new GroupBuilder().WithId(Guid.NewGuid()).WithName("group name").Build()
        );
        var id = Guid.NewGuid();

        var result = await Application.SendAsync(new DeleteGroup.Request(id));

        result.IsError.ShouldBeTrue();
        result
            .ErrorsOrEmptyList.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(Group.Id))
            );

        // verify groups are untouched
        var groupCount = await Application.CountAsync<Group>();
        groupCount.ShouldBe(1);
    }

    [Test]
    public async Task UserIdNotAGroupMember_ReturnsNotFoundError()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(new GroupBuilder().WithId(groupId).WithDefaultMember().Build());
        Application.SetUserId(TestMembers.Bob.UserId);

        var result = await Application.SendAsync(new DeleteGroup.Request(groupId));
        result.IsError.ShouldBeTrue();
        result
            .ErrorsOrEmptyList.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(Group.Id))
            );
    }

    [Test]
    public async Task DeletesGroup()
    {
        var id = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder().WithId(id).WithDefaultMember().WithName("group name").Build()
        );

        var result = await Application.SendAsync(new DeleteGroup.Request(id));

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Deleted>();

        var groupCount = await Application.CountAsync<Group>();
        groupCount.ShouldBe(0);
        var groupMemberCount = await Application.CountAsync<GroupMember>();
        groupMemberCount.ShouldBe(0);
    }

    [Test]
    public async Task DeletesCorrectGroup()
    {
        var id = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder().WithId(id).WithDefaultMember().WithName("group name").Build(),
            new GroupBuilder()
                .WithId(Guid.NewGuid())
                .WithDefaultMember()
                .WithName("other group name")
                .Build()
        );

        var result = await Application.SendAsync(new DeleteGroup.Request(id));

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Deleted>();

        var groupCount = await Application.CountAsync<Group>();
        groupCount.ShouldBe(1);
        var group1 = await Application.FindAsync<Group>(g => g.Id == id);
        group1.ShouldBeNull();
        var group2 = await Application.FindAsync<Group>(g => true);
        group2.ShouldNotBeNull();
        var groupMemberCount = await Application.CountAsync<GroupMember>();
        groupMemberCount.ShouldBe(1);
    }
}
