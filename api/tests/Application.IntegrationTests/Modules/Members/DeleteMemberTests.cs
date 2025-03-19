using ErrorOr;
using Shouldly;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Modules.Members;
using SplitTheBill.Domain.Models.Members;

namespace SplitTheBill.Application.IntegrationTests.Modules.Members;

internal sealed class DeleteMemberTests : ApplicationTestBase
{
    [Test]
    public async Task InvalidRequest_ReturnsValidationError()
    {
        var request = new DeleteMember.Request(Guid.Empty);
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
    public async Task MemberDoesNotExist_ReturnsNotFoundError()
    {
        await Application.AddAsync(Tests.Shared.TestData.Members.Alice);
        Guid id = new("3B99BDEE-3507-4D28-8593-663405D0CDA6");

        var result = await Application.SendAsync(new DeleteMember.Request(id));

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(Member.Id))
            );

        // verify members are untouched
        var memberCount = await Application.CountAsync<Member>();
        memberCount.ShouldBe(1);
    }

    [Test]
    public async Task DeletesMember()
    {
        await Application.AddAsync(Tests.Shared.TestData.Members.Alice);

        var result = await Application.SendAsync(new DeleteMember.Request(Tests.Shared.TestData.Members.Alice.Id));

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Deleted>();

        var memberCount = await Application.CountAsync<Member>();
        memberCount.ShouldBe(0);
    }

    [Test]
    public async Task DeletesCorrectMember()
    {
        await Application.AddAsync(
            Tests.Shared.TestData.Members.Alice,
            Tests.Shared.TestData.Members.Bob
        );

        var result = await Application.SendAsync(
            new DeleteMember.Request(Tests.Shared.TestData.Members.Alice.Id)
        );

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Deleted>();

        // make sure only Alice was deleted
        var memberCount = await Application.CountAsync<Member>();
        memberCount.ShouldBe(1);
        var alice = await Application.FindAsync<Member>(m => m.Id == Tests.Shared.TestData.Members.Alice.Id);
        alice.ShouldBeNull();
        var bob = await Application.FindAsync<Member>(m => m.Id == Tests.Shared.TestData.Members.Bob.Id);
        bob.ShouldNotBeNull();
    }
}