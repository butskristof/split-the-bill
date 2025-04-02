using SplitTheBill.Application.Tests.Shared.TestData;
using TUnit.Core.Interfaces;

namespace SplitTheBill.Application.IntegrationTests.Common;

[ParallelLimiter<ApplicationTestBaseParallelLimiter>]
internal abstract class ApplicationTestBase
{
    private readonly bool _seedMembers;

    [ClassDataSource<ApplicationFixture>(Shared = SharedType.PerTestSession)]
    public required ApplicationFixture Application { get; init; }

    protected ApplicationTestBase(bool seedMembers = true)
    {
        _seedMembers = seedMembers;
    }

    [Before(Test)]
    public async Task ResetStateAsync()
    {
        // reset the application state before every single test
        await Application.ResetStateAsync();
        if (_seedMembers) await SeedMembersAsync();
    }

    protected Task SeedMembersAsync() => Application.AddAsync(TestMembers.GetAllMembers());
}

internal sealed record ApplicationTestBaseParallelLimiter : IParallelLimit
{
    public int Limit => 1;
}