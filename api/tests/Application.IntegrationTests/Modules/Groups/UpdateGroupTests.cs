using ErrorOr;
using Shouldly;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Tests.Shared.Builders;
using SplitTheBill.Application.Tests.Shared.TestData;
using SplitTheBill.Application.Tests.Shared.TestData.Builders;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.IntegrationTests.Modules.Groups;

internal sealed class UpdateGroupTests : ApplicationTestBase
{
    [Test]
    public async Task InvalidRequest_ReturnsValidationErrors()
    {
        var request = new UpdateGroupRequestBuilder()
            .WithId(Guid.Empty)
            .WithName(null)
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList.Count.ShouldBe(2);
        result.ErrorsOrEmptyList
            .ShouldContain(e =>
                e.Type == ErrorType.Validation &&
                e.Code == nameof(request.Id) &&
                e.Description == ErrorCodes.Invalid);
        result.ErrorsOrEmptyList
            .ShouldContain(e =>
                e.Type == ErrorType.Validation &&
                e.Code == nameof(request.Name) &&
                e.Description == ErrorCodes.Required);
    }

    [Test]
    public async Task LongName_ReturnsValidationError()
    {
        var name = TestUtilities.GenerateString(513);
        var request = new UpdateGroupRequestBuilder()
            .WithName(name)
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .Where(e => e.Code == nameof(request.Name))
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.Validation),
                e => e.Code.ShouldBe(nameof(request.Name)),
                e => e.Description.ShouldBe(ErrorCodes.TooLong)
            );
    }

    [Test]
    public async Task GroupDoesNotExist_ReturnsNotFoundError()
    {
        await Application.AddAsync(new GroupBuilder()
            .WithId(new("DA763152-0CED-4CE7-A2A4-82BE905CDC01"))
            .WithName("group name")
            .Build()
        );
        var request = new UpdateGroupRequestBuilder()
            .WithId(new("6124BCA9-E31A-4A2B-9A7A-DA6A3BDFE78F"))
            .WithName("other group name")
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.Id))
            );
    }

    [Test]
    public async Task ValidRequest_ReturnsUpdated()
    {
        Guid id = new("638C4DA3-8592-4577-AA95-A4D103F5AD2B");
        await Application.AddAsync(new GroupBuilder()
            .WithId(id)
            .WithName("group name")
            .Build()
        );
        
        var name = TestUtilities.GenerateString(512); // max string length
        var request = new UpdateGroupRequestBuilder()
            .WithId(id)
            .WithName(name)
            .Build();
        var result = await Application.SendAsync(request);
        
        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Updated>();
        
        var group = await Application.FindAsync<Group>(g => g.Id == id);
        group!.Name.ShouldBe(name);
    }

    [Test]
    public async Task NoChanges_ReturnsUpdated()
    {
        Guid id = new("638C4DA3-8592-4577-AA95-A4D103F5AD2B");
        const string name = "group name";
        await Application.AddAsync(new GroupBuilder()
            .WithId(id)
            .WithName(name)
            .Build()
        );
        
        var request = new UpdateGroupRequestBuilder()
            .WithId(id)
            .WithName(name)
            .Build();
        var result = await Application.SendAsync(request);
        
        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Updated>();
    }
}