using ErrorOr;
using Shouldly;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.Modules.Groups.Expenses;
using SplitTheBill.Application.Tests.Shared.TestData;
using SplitTheBill.Application.Tests.Shared.TestData.Builders;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.IntegrationTests.Modules.Groups.Expenses;

internal sealed class DeleteExpenseTests : ApplicationTestBase
{
    [Test]
    public async Task InvalidRequest_ReturnsValidationError()
    {
        var request = new DeleteExpense.Request(Guid.Empty, Guid.NewGuid());
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result
            .ErrorsOrEmptyList.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.Validation),
                e => e.Code.ShouldBe(nameof(request.GroupId)),
                e => e.Description.ShouldBe(ErrorCodes.Invalid)
            );
    }

    [Test]
    public async Task GroupDoesNotExist_ReturnsNotFoundError()
    {
        var expense = new ExpenseBuilder()
            .WithId(Guid.NewGuid())
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .WithAmount(100m)
            .WithEvenSplit([TestMembers.Alice.Id])
            .Build();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(Guid.NewGuid())
                .WithMembers([TestMembers.Alice.Id])
                .WithExpenses([expense])
                .Build()
        );

        var request = new DeleteExpense.Request(Guid.NewGuid(), Guid.NewGuid());
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result
            .ErrorsOrEmptyList.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.GroupId))
            );

        // verify expenses are untouched
        var expenseCount = await Application.CountAsync<Expense>();
        expenseCount.ShouldBe(1);
        var expenseParticipantCount = await Application.CountAsync<ExpenseParticipant>();
        expenseParticipantCount.ShouldBe(1);
    }

    [Test]
    public async Task ExpenseDoesNotExist_ReturnsNotFoundError()
    {
        var expense = new ExpenseBuilder()
            .WithId(Guid.NewGuid())
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .WithAmount(100m)
            .WithEvenSplit([TestMembers.Alice.Id])
            .Build();
        var group = new GroupBuilder()
            .WithId(Guid.NewGuid())
            .WithMembers([TestMembers.Alice.Id])
            .WithExpenses([expense])
            .Build();
        await Application.AddAsync(group);
        Application.SetUserId(TestMembers.Alice.UserId);

        var request = new DeleteExpense.Request(group.Id, Guid.NewGuid());
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result
            .ErrorsOrEmptyList.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.ExpenseId))
            );

        // verify expenses are untouched
        var expenseCount = await Application.CountAsync<Expense>();
        expenseCount.ShouldBe(1);
        var expenseParticipantCount = await Application.CountAsync<ExpenseParticipant>();
        expenseParticipantCount.ShouldBe(1);
    }

    [Test]
    public async Task ExpenseExistsInOtherGroup_ReturnsNotFoundError()
    {
        var expense = new ExpenseBuilder()
            .WithId(Guid.NewGuid())
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .WithAmount(100m)
            .WithEvenSplit([TestMembers.Alice.Id])
            .Build();
        var group1 = new GroupBuilder()
            .WithId(Guid.NewGuid())
            .WithDefaultMember()
            .WithMembers([TestMembers.Alice.Id])
            .WithExpenses([expense])
            .Build();
        var group2Id = Guid.NewGuid();
        await Application.AddAsync(group1, new GroupBuilder().WithId(group2Id).WithDefaultMember());

        var request = new DeleteExpense.Request(group2Id, expense.Id);
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result
            .ErrorsOrEmptyList.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.ExpenseId))
            );

        // verify expenses are untouched
        var expenseCount = await Application.CountAsync<Expense>();
        expenseCount.ShouldBe(1);
        var expenseParticipantCount = await Application.CountAsync<ExpenseParticipant>();
        expenseParticipantCount.ShouldBe(1);
    }

    [Test]
    public async Task UserIdNotAGroupMember_ReturnsNotFoundError()
    {
        var groupId = Guid.NewGuid();
        var expense = new ExpenseBuilder()
            .WithId(Guid.NewGuid())
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .WithAmount(100m)
            .WithEvenSplit([TestMembers.Alice.Id])
            .Build();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id])
                .WithExpenses([expense])
                .Build()
        );
        Application.SetUserId(TestMembers.Bob.UserId);

        var request = new DeleteExpense.Request(groupId, expense.Id);
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result
            .ErrorsOrEmptyList.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.GroupId))
            );
    }

    [Test]
    public async Task DeletesExpense()
    {
        var groupId = Guid.NewGuid();
        var expense = new ExpenseBuilder()
            .WithId(Guid.NewGuid())
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .WithAmount(100m)
            .WithEvenSplit([TestMembers.Alice.Id])
            .Build();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id])
                .WithExpenses([expense])
                .Build()
        );
        Application.SetUserId(TestMembers.Alice.UserId);

        var request = new DeleteExpense.Request(groupId, expense.Id);
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Deleted>();

        // verify expense is deleted
        var expenseCount = await Application.CountAsync<Expense>();
        expenseCount.ShouldBe(0);
        var expenseParticipantCount = await Application.CountAsync<ExpenseParticipant>();
        expenseParticipantCount.ShouldBe(0);
    }

    [Test]
    public async Task DeletesCorrectExpense()
    {
        var groupId = Guid.NewGuid();
        var expense1 = new ExpenseBuilder()
            .WithId(Guid.NewGuid())
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .WithAmount(100m)
            .WithEvenSplit([TestMembers.Alice.Id])
            .Build();
        var expense2 = new ExpenseBuilder()
            .WithId(Guid.NewGuid())
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .WithAmount(100m)
            .WithEvenSplit([TestMembers.Alice.Id])
            .Build();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id])
                .WithExpenses([expense1, expense2])
                .Build()
        );
        Application.SetUserId(TestMembers.Alice.UserId);

        var request = new DeleteExpense.Request(groupId, expense1.Id);
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Deleted>();

        // verify expense is deleted
        var expenseCount = await Application.CountAsync<Expense>();
        expenseCount.ShouldBe(1);
        var remainingExpense = await Application.FindAsync<Expense>(expense2.Id);
        remainingExpense.ShouldNotBeNull();
    }

    [Test]
    public async Task IsRemovedFromGroupDtoAndUpdatesTotal()
    {
        var groupId = Guid.NewGuid();
        var expense1 = new ExpenseBuilder()
            .WithId(Guid.NewGuid())
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .WithAmount(100m)
            .WithEvenSplit([TestMembers.Alice.Id])
            .Build();
        var expense2 = new ExpenseBuilder()
            .WithId(Guid.NewGuid())
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .WithAmount(100m)
            .WithEvenSplit([TestMembers.Alice.Id])
            .Build();
        expense2.SetAmountAndParticipantsWithEvenSplit(
            100m,
            new HashSet<Guid> { TestMembers.Alice.Id }
        );
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id])
                .WithExpenses([expense1, expense2])
                .Build()
        );
        Application.SetUserId(TestMembers.Alice.UserId);

        var request = new DeleteExpense.Request(groupId, expense1.Id);
        await Application.SendAsync(request);

        var result = await Application.SendAsync(new GetGroup.Request(groupId));
        var response = result.Value;
        response.TotalExpenseAmount.ShouldBe(100m);
        response
            .Expenses.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(e => e.Id.ShouldBe(expense2.Id));
    }
}
