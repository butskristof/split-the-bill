using Shouldly;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Modules.Members;
using SplitTheBill.Application.Tests.Shared.TestData;

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
            TestDataMembers.Alice.Entity()
        );

        var result = await Application.SendAsync(new GetMembers.Request());

        result.IsError.ShouldBeFalse();
        var response = result.Value;
        response.ShouldNotBeNull();
        response.Members.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                m => m.Id.ShouldBe(TestDataMembers.Alice.Id),
                m => m.Name.ShouldBe(TestDataMembers.Alice.Name)
            );
    }

    [Test]
    public async Task MultipleEntities_ReturnsMappedEntities()
    {
        await Application.AddAsync(
            TestDataMembers.Alice.Entity(),
            TestDataMembers.Bob.Entity()
        );

        var result = await Application.SendAsync(new GetMembers.Request());

        result.IsError.ShouldBeFalse();
        var response = result.Value;
        response.ShouldNotBeNull();
        response.Members.Count.ShouldBe(2);
        response.Members.ShouldContain(m =>
            m.Id == TestDataMembers.Alice.Id &&
            m.Name == TestDataMembers.Alice.Name);
        response.Members.ShouldContain(m =>
            m.Id == TestDataMembers.Bob.Id &&
            m.Name == TestDataMembers.Bob.Name);
    }
}