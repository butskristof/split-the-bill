using ErrorOr;
using Shouldly;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Modules.Groups.Expenses;
using SplitTheBill.Application.Tests.Shared.Builders;
using SplitTheBill.Application.Tests.Shared.TestData;
using SplitTheBill.Application.Tests.Shared.TestData.Builders;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.IntegrationTests.Modules.Groups.Expenses;

internal sealed class UpdateExpenseTests() : ApplicationTestBase(true)
{
    [Test]
    public async Task InvalidRequest_ReturnsValidationErrors()
    {
        var request = new ExpenseRequestBuilder()
            .WithGroupId(Guid.Empty)
            .WithExpenseId(null)
            .BuildUpdateRequest();
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
                r.Code == nameof(request.ExpenseId) &&
                r.Description == ErrorCodes.Required);
    }

    [Test]
    public async Task GroupDoesNotExist_ReturnsNotFoundError()
    {
        true.ShouldBeFalse();
    }

    [Test]
    public async Task PaidByMemberDoesNotExist_ReturnsNotFoundError()
    {
        true.ShouldBeFalse();
    }

    [Test]
    public async Task PaidByMemberNotInGroup_ReturnsNotfoundError()
    {
        true.ShouldBeFalse();
    }

    [Test]
    public async Task ParticipantMemberDoesNotExist_ReturnsNotfoundError()
    {
        true.ShouldBeFalse();
    }

    [Test]
    public async Task ParticipantMemberNotInGroup_ReturnsNotfoundError()
    {
        true.ShouldBeFalse();
    }

    [Test]
    public async Task DuplicateParticipants_ReturnsValidationError()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id, TestMembers.Bob.Id])
                .Build()
        );

        var request = new ExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithAmount(200m)
            .WithSplitType(ExpenseSplitType.ExactAmount)
            .WithParticipants(new List<UpdateExpense.Request.Participant>
            {
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Alice.Id)
                    .WithExactShare(80),
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Alice.Id)
                    .WithExactShare(120)
            })
            .BuildUpdateRequest();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.Validation),
                e => e.Code.ShouldBe(nameof(request.Participants)),
                e => e.Description.ShouldBe(ErrorCodes.NotUnique)
            );
    }

    [Test]
    public async Task ValidExpenseRequest_SplitTypeEvenly()
    {
        true.ShouldBeFalse();
    }

    [Test]
    public async Task ValidExpenseRequest_SplitTypePercentual()
    {
        true.ShouldBeFalse();
    }

    [Test]
    public async Task ValidExpenseRequest_SplitTypeExactAmount()
    {
        true.ShouldBeFalse();
    }

    [Test]
    public async Task ValidExpenseRequest_DoesNotCreateDuplicateParticipants()
    {
        true.ShouldBeFalse();
    }
}