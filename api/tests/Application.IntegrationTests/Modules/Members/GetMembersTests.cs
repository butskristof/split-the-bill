using Shouldly;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Modules.Members;

namespace SplitTheBill.Application.IntegrationTests.Modules.Members;

internal sealed class GetMembersTests : ApplicationTestBase
{
    [Test]
    public async Task NoEntities_ReturnsEmptyList()
    {
        var result = await Application.SendAsync(new GetMembers.Request());

        result.IsError.ShouldBeFalse();
        var response = result.Value;
        response.ShouldNotBeNull();
        response.Members.ShouldBeEmpty();
    }

    [Test]
    public async Task SingleEntity_ReturnsMappedEntity()
    {
        await Application.AddAsync(
            Tests.Shared.TestData.Members.Alice.Entity()
        );

        var result = await Application.SendAsync(new GetMembers.Request());

        result.IsError.ShouldBeFalse();
        var response = result.Value;
        response.ShouldNotBeNull();
        response.Members.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                m => m.Id.ShouldBe(Tests.Shared.TestData.Members.Alice.Id),
                m => m.Name.ShouldBe(Tests.Shared.TestData.Members.Alice.Name)
                );
    }
}