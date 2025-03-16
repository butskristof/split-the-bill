using TUnit.Core.Interfaces;

namespace SplitTheBill.Application.IntegrationTests.Common;

[ParallelLimiter<ApplicationTestBaseParallelLimiter>]
internal abstract class ApplicationTestBase
{
    [ClassDataSource<ApplicationFixture>(Shared = SharedType.PerTestSession)]
    public required ApplicationFixture Application { get; init; }

    [Before(Test)]
    public async Task ResetStateAsync()
    {
        // reset the application state before every single test
        await Application.ResetStateAsync();
    }
}

internal sealed record ApplicationTestBaseParallelLimiter : IParallelLimit
{
    public int Limit => 1;
}