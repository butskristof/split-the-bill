using ErrorOr;
using Shouldly;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Tests.Shared.Builders;

namespace SplitTheBill.Application.IntegrationTests.Modules.Groups.Expenses;

internal sealed class CreateExpenseTests : ApplicationTestBase
{
    [Test]
    public async Task InvalidRequest_ReturnsValidationErrors()
    {
        var request = new CreateExpenseRequestBuilder()
            .WithGroupId(Guid.Empty)
            .WithPaidByMemberId(null)
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList.ShouldNotBeEmpty();
        result.ErrorsOrEmptyList
            .ShouldContain(r =>
                r.Type == ErrorType.Validation &&
                r.Code == nameof(request.GroupId) &&
                r.Description == ErrorCodes.Invalid);
        result.ErrorsOrEmptyList
            .ShouldContain(r =>
                r.Type == ErrorType.Validation &&
                r.Code == nameof(request.PaidByMemberId) &&
                r.Description == ErrorCodes.Required);
    }
}