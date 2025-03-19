using ErrorOr;
using Shouldly;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Modules.Groups;
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
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.Validation),
                e => e.Code.ShouldBe(nameof(request.Id)),
                e => e.Description.ShouldBe(ErrorCodes.Invalid)
            );
    }

    [Test]
    public async Task GroupDoesNotExist_ReturnsNotFoundError()
    {
        await Application.AddAsync(new GroupBuilder()
            .WithId(new Guid("92C569A3-6AC7-4ABC-B193-5911DFB0EC6A"))
            .WithName("group name")
            .Build());
        Guid id = new("3B99BDEE-3507-4D28-8593-663405D0CDA6");

        var result = await Application.SendAsync(new DeleteGroup.Request(id));

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(Group.Id))
            );

        // verify groups are untouched
        var groupCount = await Application.CountAsync<Group>();
        groupCount.ShouldBe(1);
    }

    [Test]
    public async Task DeletesGroup()
    {
        Guid id = new("061FAF7F-6F9C-46FD-A93D-7DCC7FBD76F6");
        await Application.AddAsync(new GroupBuilder()
            .WithId(id)
            .WithName("group name")
            .Build()
        );

        var result = await Application.SendAsync(new DeleteGroup.Request(id));

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Deleted>();

        var groupCount = await Application.CountAsync<Group>();
        groupCount.ShouldBe(0);
    }

    [Test]
    public async Task DeletesCorrectGroup()
    {
        Guid id = new("194D10ED-9222-44BB-8969-938EF19AF241");
        await Application.AddAsync(new GroupBuilder()
                .WithId(id)
                .WithName("group name")
                .Build(),
            new GroupBuilder()
                .WithId(new Guid("31C16086-2156-4828-8259-8C9282B23B34"))
                .WithName("other group name")
                .Build()
        );

        var result = await Application.SendAsync(new DeleteGroup.Request(id));

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Deleted>();

        var groupCount = await Application.CountAsync<Group>();
        groupCount.ShouldBe(1);
        var alpha = await Application.FindAsync<Group>(g => g.Id == id);
        alpha.ShouldBeNull();
        var beta = await Application.FindAsync<Group>(g => true);
        beta.ShouldNotBeNull();
    }
}