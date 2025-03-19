using ErrorOr;
using Shouldly;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Tests.Shared.Builders;
using SplitTheBill.Application.Tests.Shared.TestData;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.IntegrationTests.Modules.Groups;

internal sealed class CreateGroupTests : ApplicationTestBase
{
    [Test]
    public async Task InvalidRequest_ReturnsValidationErrors()
    {
        var request = new CreateGroupRequestBuilder()
            .WithName(string.Empty)
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.Validation),
                e => e.Code.ShouldBe(nameof(request.Name)),
                e => e.Description.ShouldBe(ErrorCodes.Required)
            );
    }

    [Test]
    public async Task LongName_ReturnsValidationError()
    {
        var name = TestUtilities.GenerateString(513);
        var request = new CreateGroupRequestBuilder()
            .WithName(name)
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.Validation),
                e => e.Code.ShouldBe(nameof(request.Name)),
                e => e.Description.ShouldBe(ErrorCodes.TooLong)
            );
    }

    [Test]
    public async Task ValidRequest_ReturnsResponseWithId()
    {
        var name = TestUtilities.GenerateString(512); // max string length
        var request = new CreateGroupRequestBuilder()
            .WithName(name)
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeFalse();
        var response = result.Value;
        response.ShouldNotBeNull();

        (await Application.CountAsync<Group>()).ShouldBe(1);
        var group = await Application.FindAsync<Group>(g => g.Id == response.Id);
        group.ShouldNotBeNull();
        group.Name.ShouldBe(name);
    }

    [Test]
    [Skip("Auth not implemented yet")]
    public async Task ValidRequest_AddsCurrentMemberToGroup()
    {
        var request = new CreateGroupRequestBuilder()
            .WithName("group name")
            .Build();
        var result = await Application.SendAsync(request);
        var id = result.Value.Id;

        var group = await Application.FindAsync<Group>(g => g.Id == id, g => g.Members);
        group!.Members
            .ShouldHaveSingleItem();
    }
}