namespace SplitTheBill.Application.IntegrationTests.Common;

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