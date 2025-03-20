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
            TestMembers.Alice
        );

        var result = await Application.SendAsync(new GetMembers.Request());

        result.IsError.ShouldBeFalse();
        var response = result.Value;
        response.ShouldNotBeNull();
        response.Members.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                m => m.Id.ShouldBe(TestMembers.Alice.Id),
                m => m.Name.ShouldBe(TestMembers.Alice.Name)
            );
    }

    [Test]
    public async Task MultipleEntities_ReturnsMappedEntities()
    {
        await Application.AddAsync(
            TestMembers.Alice,
            TestMembers.Bob
        );

        var result = await Application.SendAsync(new GetMembers.Request());

        result.IsError.ShouldBeFalse();
        var response = result.Value;
        response.ShouldNotBeNull();
        response.Members.Count.ShouldBe(2);
        response.Members.ShouldContain(m =>
            m.Id == TestMembers.Alice.Id &&
            m.Name == TestMembers.Alice.Name);
        response.Members.ShouldContain(m =>
            m.Id == TestMembers.Bob.Id &&
            m.Name == TestMembers.Bob.Name);
    }
}