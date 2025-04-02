using ErrorOr;
using Shouldly;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.Modules.Groups.Expenses;
using SplitTheBill.Application.Tests.Shared.Builders;
using SplitTheBill.Application.Tests.Shared.TestData;
using SplitTheBill.Application.Tests.Shared.TestData.Builders;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.IntegrationTests.Modules.Groups.Expenses;

internal sealed class CreateExpenseTests : ApplicationTestBase
{
    [Test]
    public async Task InvalidRequest_ReturnsValidationErrors()
    {
        var request = new ExpenseRequestBuilder()
            .WithGroupId(Guid.Empty)
            .WithPaidByMemberId(null)
            .BuildCreateRequest();
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

    [Test]
    public async Task GroupDoesNotExist_ReturnsNotFoundError()
    {
        var request = new ExpenseRequestBuilder()
            .WithGroupId(Guid.NewGuid())
            .BuildCreateRequest();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.GroupId))
            );
    }

    [Test]
    public async Task UserIdNotAGroupMember_ReturnsNotFoundError()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id])
                .Build()
        );
        Application.SetUserId(TestMembers.Bob.UserId);
        
        var request = new ExpenseRequestBuilder()
            .WithGroupId(groupId)
            .BuildCreateRequest();
        
        var result = await Application.SendAsync(request);
        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.GroupId))
            );
    }

    [Test]
    public async Task PaidByMemberDoesNotExist_ReturnsNotFoundError()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithDefaultMember()
                .Build()
        );
        var request = new ExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithPaidByMemberId(Guid.NewGuid())
            .BuildCreateRequest();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.PaidByMemberId))
            );
    }

    [Test]
    public async Task PaidByMemberNotInGroup_ReturnsNotFoundError()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithDefaultMember()
                .Build()
        );

        var request = new ExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .BuildCreateRequest();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.PaidByMemberId))
            );
    }

    [Test]
    public async Task ParticipantMemberDoesNotExist_ReturnsNotFoundError()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id])
                .Build()
        );
        Application.SetUserId(TestMembers.Alice.UserId);
        
        var request = new ExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .WithParticipants(new List<CreateExpense.Request.Participant>
            {
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(Guid.NewGuid())
            })
            .BuildCreateRequest();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(
                    $"{nameof(request.Participants)}[0].{nameof(CreateExpense.Request.Participant.MemberId)}"
                )
            );
    }

    [Test]
    public async Task ParticipantMemberNotInGroup_ReturnsNotFoundError()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id])
                .Build()
        );
        Application.SetUserId(TestMembers.Alice.UserId);
        
        var request = new ExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .WithParticipants(new List<CreateExpense.Request.Participant>
            {
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Bob.Id)
            })
            .BuildCreateRequest();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(
                    $"{nameof(request.Participants)}[0].{nameof(CreateExpense.Request.Participant.MemberId)}"
                )
            );
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
            .WithAmount(200)
            .WithSplitType(ExpenseSplitType.ExactAmount)
            .WithParticipants(new List<CreateExpense.Request.Participant>
            {
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Alice.Id)
                    .WithExactShare(80),
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Alice.Id)
                    .WithExactShare(120)
            })
            .BuildCreateRequest();
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
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id, TestMembers.Bob.Id])
                .Build()
        );
        Application.SetUserId(TestMembers.Alice.UserId);

        var timestamp = new DateTimeOffset(2025, 04, 03, 00, 47, 40, TimeSpan.Zero);
        const string description = "Some fancy expense";
        const decimal amount = 200.00m;
        var request = new ExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithDescription(description)
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .WithTimestamp(timestamp)
            .WithAmount(amount)
            .WithSplitType(ExpenseSplitType.Evenly)
            .WithParticipants(new List<CreateExpense.Request.Participant>
            {
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Alice.Id),
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Bob.Id)
            })
            .BuildCreateRequest();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Created>();

        var expense = await Application
            .FindAsync<Expense>(e => e.GroupId == groupId,
                e => e.Participants
            );
        expense
            .ShouldNotBeNull()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe(description),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Alice.Id),
                e => e.Timestamp.ShouldBe(timestamp),
                e => e.Amount.ShouldBe(amount),
                e => e.SplitType.ShouldBe(ExpenseSplitType.Evenly),
                e => e.Participants.Count.ShouldBe(2),
                e => e.Participants.ShouldAllBe(p => p.PercentualShare == null && p.ExactShare == null),
                e => e.Participants.ShouldContain(p => p.MemberId == TestMembers.Alice.Id),
                e => e.Participants.ShouldContain(p => p.MemberId == TestMembers.Bob.Id)
            );

        var groupDto = await Application.SendAsync(new GetGroup.Request(groupId));
        var response = groupDto.Value;
        response.TotalExpenseAmount.ShouldBe(amount);
        response.Expenses
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe(description),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Alice.Id),
                e => e.Timestamp.ShouldBe(timestamp),
                e => e.Amount.ShouldBe(amount),
                e => e.SplitType.ShouldBe(ExpenseSplitType.Evenly),
                e => e.Participants.Count.ShouldBe(2),
                e => e.Participants.ShouldAllBe(p => p.PercentualShare == null && p.ExactShare == null),
                e => e.Participants.ShouldContain(p => p.MemberId == TestMembers.Alice.Id),
                e => e.Participants.ShouldContain(p => p.MemberId == TestMembers.Bob.Id)
            );
    }

    [Test]
    public async Task ValidExpenseRequest_SplitTypePercentual()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id, TestMembers.Bob.Id])
                .Build()
        );
        Application.SetUserId(TestMembers.Alice.UserId);

        const string description = "Some fancy expense";
        const decimal amount = 200.00m;
        var timestamp = new DateTimeOffset(2025, 04, 03, 00, 50, 39, TimeSpan.Zero);
        var request = new ExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithDescription(description)
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .WithTimestamp(timestamp)
            .WithAmount(amount)
            .WithSplitType(ExpenseSplitType.Percentual)
            .WithParticipants(new List<CreateExpense.Request.Participant>
            {
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Alice.Id)
                    .WithPercentualShare(60),
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Bob.Id)
                    .WithPercentualShare(40)
            })
            .BuildCreateRequest();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Created>();

        var expense = await Application
            .FindAsync<Expense>(g => g.GroupId == groupId,
                g => g.Participants
            );
        expense
            .ShouldNotBeNull()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe(description),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Alice.Id),
                e => e.Timestamp.ShouldBe(timestamp),
                e => e.Amount.ShouldBe(amount),
                e => e.SplitType.ShouldBe(ExpenseSplitType.Percentual),
                e => e.Participants.Count.ShouldBe(2),
                e => e.Participants.ShouldAllBe(p => p.ExactShare == null),
                e => e.Participants
                    .ShouldContain(p => p.MemberId == TestMembers.Alice.Id && p.PercentualShare == 60),
                e => e.Participants
                    .ShouldContain(p => p.MemberId == TestMembers.Bob.Id && p.PercentualShare == 40)
            );

        var groupDto = await Application.SendAsync(new GetGroup.Request(groupId));
        var response = groupDto.Value;
        response.TotalExpenseAmount.ShouldBe(amount);
        response.Expenses
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe(description),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Alice.Id),
                e => e.Timestamp.ShouldBe(timestamp),
                e => e.Amount.ShouldBe(amount),
                e => e.SplitType.ShouldBe(ExpenseSplitType.Percentual),
                e => e.Participants.Count.ShouldBe(2),
                e => e.Participants.ShouldAllBe(p => p.ExactShare == null),
                e => e.Participants
                    .ShouldContain(p => p.MemberId == TestMembers.Alice.Id && p.PercentualShare == 60),
                e => e.Participants
                    .ShouldContain(p => p.MemberId == TestMembers.Bob.Id && p.PercentualShare == 40)
            );
    }

    [Test]
    public async Task ValidExpenseRequest_SplitTypeExactAmount()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id, TestMembers.Bob.Id])
                .Build()
        );
        Application.SetUserId(TestMembers.Alice.UserId);

        var timestamp = new DateTimeOffset(2025, 04, 03, 00, 51, 00, TimeSpan.Zero);
        const string description = "Some fancy expense";
        const decimal amount = 200.00m;
        var request = new ExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithDescription(description)
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .WithTimestamp(timestamp)
            .WithAmount(amount)
            .WithSplitType(ExpenseSplitType.ExactAmount)
            .WithParticipants(new List<CreateExpense.Request.Participant>
            {
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Alice.Id)
                    .WithExactShare(120),
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Bob.Id)
                    .WithExactShare(80)
            })
            .BuildCreateRequest();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Created>();

        var expense = await Application
            .FindAsync<Expense>(g => g.GroupId == groupId,
                g => g.Participants
            );
        expense
            .ShouldNotBeNull()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe(description),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Alice.Id),
                e => e.Timestamp.ShouldBe(timestamp),
                e => e.Amount.ShouldBe(amount),
                e => e.SplitType.ShouldBe(ExpenseSplitType.ExactAmount),
                e => e.Participants.Count.ShouldBe(2),
                e => e.Participants.ShouldAllBe(p => p.PercentualShare == null),
                e => e.Participants
                    .ShouldContain(p => p.MemberId == TestMembers.Alice.Id && p.ExactShare == 120),
                e => e.Participants
                    .ShouldContain(p => p.MemberId == TestMembers.Bob.Id && p.ExactShare == 80)
            );

        var groupDto = await Application.SendAsync(new GetGroup.Request(groupId));
        var response = groupDto.Value;
        response.TotalExpenseAmount.ShouldBe(amount);
        response.Expenses
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe(description),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Alice.Id),
                e => e.Timestamp.ShouldBe(timestamp),
                e => e.Amount.ShouldBe(amount),
                e => e.SplitType.ShouldBe(ExpenseSplitType.ExactAmount),
                e => e.Participants.Count.ShouldBe(2),
                e => e.Participants.ShouldAllBe(p => p.PercentualShare == null),
                e => e.Participants
                    .ShouldContain(p => p.MemberId == TestMembers.Alice.Id && p.ExactShare == 120),
                e => e.Participants
                    .ShouldContain(p => p.MemberId == TestMembers.Bob.Id && p.ExactShare == 80)
            );
    }
}