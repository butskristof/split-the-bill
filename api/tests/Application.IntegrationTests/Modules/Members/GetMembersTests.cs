using Shouldly;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Modules.Members;
using SplitTheBill.Application.Tests.Shared.TestData;

namespace SplitTheBill.Application.IntegrationTests.Modules.Members;

internal sealed class GetMembersTests() : ApplicationTestBase(false)
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
    public async Task ReturnsAllMappedEntities()
    {
        await Application.AddAsync(TestMembers.Alice, TestMembers.Bob);

        var result = await Application.SendAsync(new GetMembers.Request());

        result.IsError.ShouldBeFalse();
        var response = result.Value;
        response.ShouldNotBeNull();
        response.Members.Count.ShouldBe(2);
        response.Members.ShouldContain(m =>
            m.Id == TestMembers.Alice.Id && m.Name == TestMembers.Alice.Name
        );
        response.Members.ShouldContain(m =>
            m.Id == TestMembers.Bob.Id && m.Name == TestMembers.Bob.Name
        );
    }
}
