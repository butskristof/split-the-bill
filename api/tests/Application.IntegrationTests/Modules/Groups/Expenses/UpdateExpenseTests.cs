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

internal sealed class UpdateExpenseTests : ApplicationTestBase
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
        var request = new ExpenseRequestBuilder()
            .WithGroupId(Guid.NewGuid())
            .BuildUpdateRequest();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList.ShouldNotBeEmpty();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.GroupId))
            );
    }

    [Test]
    public async Task ExpenseDoesNotExist_ReturnsNotFoundError()
    {
        var groupId = Guid.NewGuid();
        var expenseId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithDefaultMember()
                .WithExpenses([
                    new ExpenseBuilder()
                        .WithId(expenseId)
                        .WithDescription("initial description")
                        .WithAmount(200m)
                        .WithPaidByMemberId(TestMembers.Alice.Id)
                        .WithEvenSplit([TestMembers.Alice.Id])
                ])
                .Build()
        );

        var request = new ExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithExpenseId(Guid.NewGuid())
            .WithDescription("updated description")
            .WithAmount(300m)
            .WithPaidByMemberId(TestMembers.Bob.Id)
            .WithParticipants([
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Bob.Id)
                    .BuildUpdateParticipant()
            ])
            .BuildUpdateRequest();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.ExpenseId))
            );

        // verify expense is untouched
        var expense = await Application.FindAsync<Expense>(e => e.Id == expenseId,
            e => e.Participants);
        expense.ShouldNotBeNull();
        expense.Description.ShouldBe("initial description");
        expense.Amount.ShouldBe(200m);
        expense.PaidByMemberId.ShouldBe(TestMembers.Alice.Id);
        expense.SplitType.ShouldBe(ExpenseSplitType.Evenly);
        expense.Participants
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                p => p.MemberId.ShouldBe(TestMembers.Alice.Id)
            );
    }

    [Test]
    public async Task ExpenseExistsInOtherGroup_ReturnsNotFoundError()
    {
        var groupId = Guid.NewGuid();
        var expenseId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(Guid.NewGuid())
                .WithDefaultMember()
                .WithExpenses([
                    new ExpenseBuilder()
                        .WithId(expenseId)
                        .WithDescription("initial description")
                        .WithAmount(200m)
                        .WithPaidByMemberId(TestMembers.Alice.Id)
                        .WithEvenSplit([TestMembers.Alice.Id])
                ])
                .Build(),
            new GroupBuilder()
                .WithId(groupId)
                .WithDefaultMember()
                .Build()
        );

        var request = new ExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithExpenseId(expenseId)
            .WithDescription("updated description")
            .WithAmount(300m)
            .WithPaidByMemberId(TestMembers.Bob.Id)
            .WithParticipants([
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Bob.Id)
                    .BuildUpdateParticipant()
            ])
            .BuildUpdateRequest();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.ExpenseId))
            );

        // verify expense is untouched
        var expense = await Application.FindAsync<Expense>(e => e.Id == expenseId,
            e => e.Participants);
        expense.ShouldNotBeNull();
        expense.Description.ShouldBe("initial description");
        expense.Amount.ShouldBe(200m);
        expense.PaidByMemberId.ShouldBe(TestMembers.Alice.Id);
        expense.SplitType.ShouldBe(ExpenseSplitType.Evenly);
        expense.Participants
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                p => p.MemberId.ShouldBe(TestMembers.Alice.Id)
            );
    }

    [Test]
    public async Task UserIdNotAGroupMember_ReturnsNotFoundError()
    {
        var groupId = Guid.NewGuid();
        var expenseId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id, TestMembers.Bob.Id])
                .AddExpense(new ExpenseBuilder()
                    .WithId(expenseId)
                    .WithDescription("initial description")
                    .WithAmount(200m)
                    .WithPaidByMemberId(TestMembers.Alice.Id)
                    .WithEvenSplit([TestMembers.Alice.Id])
                )
                .Build()
        );
        Application.SetUserId(TestMembers.Default.UserId);
        
        var request = new ExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithExpenseId(expenseId)
            .BuildUpdateRequest();
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
        var expenseId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithDefaultMember()
                .AddExpense(new ExpenseBuilder()
                    .WithId(expenseId)
                    .WithPaidByMemberId(TestMembers.Alice.Id)
                    .WithEvenSplit([TestMembers.Alice.Id])
                )
                .Build()
        );

        var request = new ExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithExpenseId(expenseId)
            .WithPaidByMemberId(Guid.NewGuid())
            .BuildUpdateRequest();
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
        var expenseId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithDefaultMember()
                .AddExpense(new ExpenseBuilder()
                    .WithId(expenseId)
                    .WithPaidByMemberId(TestMembers.Alice.Id)
                    .WithEvenSplit([TestMembers.Alice.Id])
                )
                .Build()
        );

        var request = new ExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithExpenseId(expenseId)
            .WithPaidByMemberId(TestMembers.Bob.Id)
            .BuildUpdateRequest();
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
        var expenseId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id])
                .AddExpense(new ExpenseBuilder()
                    .WithId(expenseId)
                    .WithPaidByMemberId(TestMembers.Alice.Id)
                    .WithEvenSplit([TestMembers.Alice.Id])
                )
                .Build()
        );
        Application.SetUserId(TestMembers.Alice.UserId);

        var request = new ExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithExpenseId(expenseId)
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .WithParticipants(new List<UpdateExpense.Request.Participant>
            {
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(Guid.NewGuid())
            })
            .BuildUpdateRequest();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(
                    $"{nameof(request.Participants)}[0].{nameof(UpdateExpense.Request.Participant.MemberId)}"
                )
            );
    }

    [Test]
    public async Task ParticipantMemberNotInGroup_ReturnsNotFoundError()
    {
        var groupId = Guid.NewGuid();
        var expenseId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id])
                .AddExpense(new ExpenseBuilder()
                    .WithId(expenseId)
                    .WithPaidByMemberId(TestMembers.Alice.Id)
                    .WithEvenSplit([TestMembers.Alice.Id])
                )
                .Build()
        );
        Application.SetUserId(TestMembers.Alice.UserId);

        var request = new ExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithExpenseId(expenseId)
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .WithParticipants(new List<UpdateExpense.Request.Participant>
            {
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Bob.Id)
            })
            .BuildUpdateRequest();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(
                    $"{nameof(request.Participants)}[0].{nameof(UpdateExpense.Request.Participant.MemberId)}"
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
        var groupId = Guid.NewGuid();
        var expenseId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id, TestMembers.Bob.Id, TestMembers.Charlie.Id])
                .AddExpense(new ExpenseBuilder()
                    .WithId(expenseId)
                    .WithDescription("initial description")
                    .WithAmount(200m)
                    .WithPaidByMemberId(TestMembers.Alice.Id)
                    .WithPercentualSplit(new Dictionary<Guid, int>
                    {
                        { TestMembers.Alice.Id, 60 },
                        { TestMembers.Bob.Id, 40 }
                    })
                )
                .Build()
        );
        Application.SetUserId(TestMembers.Alice.UserId);

        var timestamp = new DateTimeOffset(2025, 04, 03, 00, 52, 15, TimeSpan.Zero);
        const string description = "updated description";
        const decimal amount = 300m;
        var request = new ExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithExpenseId(expenseId)
            .WithDescription(description)
            .WithPaidByMemberId(TestMembers.Bob.Id)
            .WithTimestamp(timestamp)
            .WithAmount(amount)
            .WithSplitType(ExpenseSplitType.Evenly)
            .WithParticipants(new List<UpdateExpense.Request.Participant>
            {
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Bob.Id),
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Charlie.Id),
            })
            .BuildUpdateRequest();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Updated>();

        var expense = await Application
            .FindAsync<Expense>(e => e.Id == expenseId,
                e => e.Participants
            );
        expense
            .ShouldNotBeNull()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe(description),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Bob.Id),
                e => e.Timestamp.ShouldBe(timestamp),
                e => e.Amount.ShouldBe(amount),
                e => e.SplitType.ShouldBe(ExpenseSplitType.Evenly),
                e => e.Participants.Count.ShouldBe(2),
                e => e.Participants.ShouldAllBe(p => p.PercentualShare == null && p.ExactShare == null),
                e => e.Participants.ShouldContain(p => p.MemberId == TestMembers.Bob.Id),
                e => e.Participants.ShouldContain(p => p.MemberId == TestMembers.Charlie.Id)
            );

        var groupDto = await Application.SendAsync(new GetGroup.Request(groupId));
        var response = groupDto.Value;
        response.TotalExpenseAmount.ShouldBe(amount);
        response.Expenses
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe(description),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Bob.Id),
                e => e.Timestamp.ShouldBe(timestamp),
                e => e.Amount.ShouldBe(amount),
                e => e.SplitType.ShouldBe(ExpenseSplitType.Evenly),
                e => e.Participants.Count.ShouldBe(2),
                e => e.Participants.ShouldAllBe(p => p.PercentualShare == null && p.ExactShare == null),
                e => e.Participants.ShouldContain(p => p.MemberId == TestMembers.Bob.Id),
                e => e.Participants.ShouldContain(p => p.MemberId == TestMembers.Charlie.Id)
            );
    }

    [Test]
    public async Task ValidExpenseRequest_SplitTypePercentual()
    {
        var groupId = Guid.NewGuid();
        var expenseId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id, TestMembers.Bob.Id, TestMembers.Charlie.Id])
                .AddExpense(new ExpenseBuilder()
                    .WithId(expenseId)
                    .WithDescription("initial description")
                    .WithAmount(200m)
                    .WithPaidByMemberId(TestMembers.Alice.Id)
                    .WithExactAmountSplit(new Dictionary<Guid, decimal>
                    {
                        { TestMembers.Alice.Id, 120m },
                        { TestMembers.Bob.Id, 80m }
                    })
                )
                .Build()
        );
        Application.SetUserId(TestMembers.Alice.UserId);

        var timestamp = new DateTimeOffset(2025, 04, 03, 00, 52, 33, TimeSpan.Zero);
        const string description = "updated description";
        const decimal amount = 300m;
        var request = new ExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithExpenseId(expenseId)
            .WithDescription(description)
            .WithPaidByMemberId(TestMembers.Bob.Id)
            .WithTimestamp(timestamp)
            .WithAmount(amount)
            .WithSplitType(ExpenseSplitType.Percentual)
            .WithParticipants(new List<UpdateExpense.Request.Participant>
            {
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Bob.Id)
                    .WithPercentualShare(60),
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Charlie.Id)
                    .WithPercentualShare(40),
            })
            .BuildUpdateRequest();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Updated>();

        var expense = await Application
            .FindAsync<Expense>(e => e.Id == expenseId,
                e => e.Participants
            );
        expense
            .ShouldNotBeNull()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe(description),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Bob.Id),
                e => e.Timestamp.ShouldBe(timestamp),
                e => e.Amount.ShouldBe(amount),
                e => e.SplitType.ShouldBe(ExpenseSplitType.Percentual),
                e => e.Participants.Count.ShouldBe(2),
                e => e.Participants.ShouldAllBe(p => p.ExactShare == null),
                e => e.Participants.ShouldContain(p => p.MemberId == TestMembers.Bob.Id && p.PercentualShare == 60),
                e => e.Participants.ShouldContain(p => p.MemberId == TestMembers.Charlie.Id && p.PercentualShare == 40)
            );

        var groupDto = await Application.SendAsync(new GetGroup.Request(groupId));
        var response = groupDto.Value;
        response.TotalExpenseAmount.ShouldBe(amount);
        response.Expenses
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe(description),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Bob.Id),
                e => e.Timestamp.ShouldBe(timestamp),
                e => e.Amount.ShouldBe(amount),
                e => e.SplitType.ShouldBe(ExpenseSplitType.Percentual),
                e => e.Participants.Count.ShouldBe(2),
                e => e.Participants.ShouldAllBe(p => p.ExactShare == null),
                e => e.Participants.ShouldContain(p => p.MemberId == TestMembers.Bob.Id && p.PercentualShare == 60),
                e => e.Participants.ShouldContain(p => p.MemberId == TestMembers.Charlie.Id && p.PercentualShare == 40)
            );
    }

    [Test]
    public async Task ValidExpenseRequest_SplitTypeExactAmount()
    {
        var groupId = Guid.NewGuid();
        var expenseId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id, TestMembers.Bob.Id, TestMembers.Charlie.Id])
                .AddExpense(new ExpenseBuilder()
                    .WithId(expenseId)
                    .WithDescription("initial description")
                    .WithAmount(200m)
                    .WithPaidByMemberId(TestMembers.Alice.Id)
                    .WithPercentualSplit(new Dictionary<Guid, int>
                    {
                        { TestMembers.Alice.Id, 60 },
                        { TestMembers.Bob.Id, 40 }
                    })
                )
                .Build()
        );
        Application.SetUserId(TestMembers.Alice.UserId);

        var timestamp = new DateTimeOffset(2025, 04, 03, 00, 52, 55, TimeSpan.Zero);
        const string description = "updated description";
        const decimal amount = 300m;
        var request = new ExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithExpenseId(expenseId)
            .WithDescription(description)
            .WithPaidByMemberId(TestMembers.Bob.Id)
            .WithTimestamp(timestamp)
            .WithAmount(amount)
            .WithSplitType(ExpenseSplitType.ExactAmount)
            .WithParticipants(new List<UpdateExpense.Request.Participant>
            {
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Bob.Id)
                    .WithExactShare(180m),
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Charlie.Id)
                    .WithExactShare(120m),
            })
            .BuildUpdateRequest();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Updated>();

        var expense = await Application
            .FindAsync<Expense>(e => e.Id == expenseId,
                e => e.Participants
            );
        expense
            .ShouldNotBeNull()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe(description),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Bob.Id),
                e => e.Timestamp.ShouldBe(timestamp),
                e => e.Amount.ShouldBe(amount),
                e => e.SplitType.ShouldBe(ExpenseSplitType.ExactAmount),
                e => e.Participants.Count.ShouldBe(2),
                e => e.Participants.ShouldAllBe(p => p.PercentualShare == null),
                e => e.Participants.ShouldContain(p => p.MemberId == TestMembers.Bob.Id && p.ExactShare == 180m),
                e => e.Participants.ShouldContain(p => p.MemberId == TestMembers.Charlie.Id && p.ExactShare == 120m)
            );

        var groupDto = await Application.SendAsync(new GetGroup.Request(groupId));
        var response = groupDto.Value;
        response.TotalExpenseAmount.ShouldBe(amount);
        response.Expenses
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe(description),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Bob.Id),
                e => e.Timestamp.ShouldBe(timestamp),
                e => e.Amount.ShouldBe(amount),
                e => e.SplitType.ShouldBe(ExpenseSplitType.ExactAmount),
                e => e.Participants.Count.ShouldBe(2),
                e => e.Participants.ShouldAllBe(p => p.PercentualShare == null),
                e => e.Participants.ShouldContain(p => p.MemberId == TestMembers.Bob.Id && p.ExactShare == 180m),
                e => e.Participants.ShouldContain(p => p.MemberId == TestMembers.Charlie.Id && p.ExactShare == 120m)
            );
    }

    [Test]
    public async Task ValidExpenseRequest_DoesNotCreateDuplicateParticipants()
    {
        var groupId = Guid.NewGuid();
        var expenseId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id, TestMembers.Bob.Id, TestMembers.Charlie.Id])
                .AddExpense(new ExpenseBuilder()
                    .WithId(expenseId)
                    .WithDescription("initial description")
                    .WithAmount(200m)
                    .WithPaidByMemberId(TestMembers.Alice.Id)
                    .WithEvenSplit([TestMembers.Alice.Id, TestMembers.Bob.Id])
                )
                .Build()
        );
        Application.SetUserId(TestMembers.Alice.UserId);

        var request = new ExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithExpenseId(expenseId)
            .WithDescription("updated description")
            .WithAmount(300m)
            .WithPaidByMemberId(TestMembers.Bob.Id)
            .WithSplitType(ExpenseSplitType.Evenly)
            .WithParticipants(new List<UpdateExpense.Request.Participant>
            {
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Bob.Id),
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Charlie.Id),
            })
            .BuildUpdateRequest();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Updated>();

        var expenseParticipants = await Application.CountAsync<ExpenseParticipant>();
        expenseParticipants.ShouldBe(2);
        var participant1 =
            await Application.FindAsync<ExpenseParticipant>(ep =>
                ep.ExpenseId == expenseId && ep.MemberId == TestMembers.Bob.Id);
        participant1.ShouldNotBeNull();
        var participant2 =
            await Application.FindAsync<ExpenseParticipant>(ep =>
                ep.ExpenseId == expenseId && ep.MemberId == TestMembers.Charlie.Id);
        participant2.ShouldNotBeNull();
    }
}