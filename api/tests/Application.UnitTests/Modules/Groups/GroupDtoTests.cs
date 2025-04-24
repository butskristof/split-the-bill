using Shouldly;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.Tests.Shared.TestData;
using SplitTheBill.Application.Tests.Shared.TestData.Builders;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.UnitTests.Modules.Groups;

internal sealed class GroupDtoTests
{
    [Test]
    public void GroupDto_MapsGroupProperties()
    {
        var id = Guid.NewGuid();
        const string name = "group name";
        var group = new GroupBuilder().WithId(id).WithName(name).Build();
        var dto = new GroupDto(group);

        dto.Id.ShouldBe(id);
        dto.Name.ShouldBe(name);
    }

    [Test]
    public void GroupDto_MapsMemberProperties()
    {
        var group = new GroupBuilder().WithMembers([TestMembers.Alice, TestMembers.Bob]).Build();
        var dto = new GroupDto(group);

        dto.Members.Count.ShouldBe(2);
        dto.Members.Where(m => m.Id == TestMembers.Alice.Id)
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(m => m.Name.ShouldBe(TestMembers.Alice.Name));
        dto.Members.Where(m => m.Id == TestMembers.Bob.Id)
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(m => m.Name.ShouldBe(TestMembers.Bob.Name));
    }

    [Test]
    public void GroupDto_MapsPaymentProperties()
    {
        var payment1Id = Guid.NewGuid();

        var group = new GroupBuilder()
            .AddPayment(
                new PaymentBuilder()
                    .WithId(payment1Id)
                    .WithSendingMemberId(TestMembers.Alice.Id)
                    .WithReceivingMemberId(TestMembers.Bob.Id)
                    .WithAmount(1m)
            )
            .Build();
        var dto = new GroupDto(group);

        dto.Payments.ShouldHaveSingleItem();
        dto.Payments.Where(p => p.Id == payment1Id)
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                p => p.SendingMemberId.ShouldBe(TestMembers.Alice.Id),
                p => p.ReceivingMemberId.ShouldBe(TestMembers.Bob.Id),
                p => p.Amount.ShouldBe(1m)
            );
    }

    [Test]
    public void GroupDto_OrdersPaymentsDescending()
    {
        var baseTimestamp = DateTimeOffset.UtcNow;
        var group = new GroupBuilder()
            .AddPayment(new PaymentBuilder().WithTimestamp(baseTimestamp.AddMinutes(1)))
            .AddPayment(new PaymentBuilder().WithTimestamp(baseTimestamp.AddMinutes(-1)))
            .AddPayment(new PaymentBuilder().WithTimestamp(baseTimestamp.AddMinutes(15)))
            .Build();
        var dto = new GroupDto(group);

        dto.Payments.Select(p => p.Timestamp).ShouldBeInOrder(SortDirection.Descending);
    }

    [Test]
    public void GroupDto_MapsExpenseProperties()
    {
        var expense1Id = Guid.NewGuid();
        var expense2Id = Guid.NewGuid();
        var expense3Id = Guid.NewGuid();

        var group = new GroupBuilder()
            .AddExpense(
                new ExpenseBuilder()
                    .WithId(expense1Id)
                    .WithDescription("expense 1")
                    .WithAmount(1)
                    .WithEvenSplit([TestMembers.Alice.Id, TestMembers.Bob.Id])
                    .WithPaidByMemberId(TestMembers.Alice.Id)
            )
            .AddExpense(
                new ExpenseBuilder()
                    .WithId(expense2Id)
                    .WithDescription("expense 2")
                    .WithAmount(2)
                    .WithPercentualSplit(
                        new Dictionary<Guid, int>
                        {
                            { TestMembers.Alice.Id, 60 },
                            { TestMembers.Bob.Id, 40 },
                        }
                    )
                    .WithPaidByMemberId(TestMembers.Bob.Id)
            )
            .AddExpense(
                new ExpenseBuilder()
                    .WithId(expense3Id)
                    .WithDescription("expense 3")
                    .WithAmount(3)
                    .WithExactAmountSplit(
                        new Dictionary<Guid, decimal>
                        {
                            { TestMembers.Alice.Id, 2 },
                            { TestMembers.Bob.Id, 1 },
                        }
                    )
                    .WithPaidByMemberId(TestMembers.Charlie.Id)
            )
            .Build();
        var dto = new GroupDto(group);

        dto.Expenses.Count.ShouldBe(3);
        dto.Expenses.Where(e => e.Id == expense1Id)
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe("expense 1"),
                e => e.Amount.ShouldBe(1),
                e => e.SplitType.ShouldBe(ExpenseSplitType.Evenly),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Alice.Id),
                e => e.Participants.Count.ShouldBe(2),
                e =>
                    e.Participants.Where(p => p.MemberId == TestMembers.Alice.Id)
                        .ShouldHaveSingleItem(),
                e =>
                    e.Participants.Where(p => p.MemberId == TestMembers.Bob.Id)
                        .ShouldHaveSingleItem()
            );
        dto.Expenses.Where(e => e.Id == expense2Id)
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe("expense 2"),
                e => e.Amount.ShouldBe(2),
                e => e.SplitType.ShouldBe(ExpenseSplitType.Percentual),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Bob.Id),
                e => e.Participants.Count.ShouldBe(2),
                e =>
                    e.Participants.Where(p => p.MemberId == TestMembers.Alice.Id)
                        .ShouldHaveSingleItem()
                        .ShouldSatisfyAllConditions(p => p.PercentualShare.ShouldBe(60)),
                e =>
                    e.Participants.Where(p => p.MemberId == TestMembers.Bob.Id)
                        .ShouldHaveSingleItem()
                        .ShouldSatisfyAllConditions(p => p.PercentualShare.ShouldBe(40))
            );
        dto.Expenses.Where(e => e.Id == expense3Id)
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe("expense 3"),
                e => e.Amount.ShouldBe(3),
                e => e.SplitType.ShouldBe(ExpenseSplitType.ExactAmount),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Charlie.Id),
                e => e.Participants.Count.ShouldBe(2),
                e =>
                    e.Participants.Where(p => p.MemberId == TestMembers.Alice.Id)
                        .ShouldHaveSingleItem()
                        .ShouldSatisfyAllConditions(p => p.ExactShare.ShouldBe(2)),
                e =>
                    e.Participants.Where(p => p.MemberId == TestMembers.Bob.Id)
                        .ShouldHaveSingleItem()
                        .ShouldSatisfyAllConditions(p => p.ExactShare.ShouldBe(1))
            );
    }

    [Test]
    public void GroupDto_OrdersExpensesDescending()
    {
        var baseTimestamp = DateTimeOffset.UtcNow;
        var group = new GroupBuilder()
            .AddExpense(
                new ExpenseBuilder()
                    .WithEvenSplit([TestMembers.Alice.Id])
                    .WithTimestamp(baseTimestamp.AddMinutes(1))
            )
            .AddExpense(
                new ExpenseBuilder()
                    .WithEvenSplit([TestMembers.Alice.Id])
                    .WithTimestamp(baseTimestamp.AddMinutes(-1))
            )
            .AddExpense(
                new ExpenseBuilder()
                    .WithEvenSplit([TestMembers.Alice.Id])
                    .WithTimestamp(baseTimestamp.AddMinutes(15))
            )
            .Build();
        var dto = new GroupDto(group);

        dto.Expenses.Select(e => e.Timestamp).ShouldBeInOrder(SortDirection.Descending);
    }
}
