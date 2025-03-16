using Shouldly;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.Tests.Shared.TestData.Builders;

namespace SplitTheBill.Application.UnitTests.Modules.Groups;

internal sealed class GroupDtoTotalsTests
{
    [Test]
    public void EmptyGroup_TotalAmountsShouldBeZero()
    {
        var group = new GroupBuilder()
            .Build();
        var dto = new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(0);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(0);
    }

    [Test]
    public void WithExpenses_ShouldSumAmounts()
    {
        var group = new GroupBuilder()
            .AddExpense(new ExpenseBuilder()
                .WithAmount(100)
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(200)
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(1234)
            )
            .Build();
        var dto = new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(1534);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(1534);
    }

    [Test]
    public void WithPayments_ShouldSumAmounts()
    {
        var group = new GroupBuilder()
            .AddPayment(new PaymentBuilder()
                .WithAmount(100)
            )
            .AddPayment(new PaymentBuilder()
                .WithAmount(200)
            )
            .AddPayment(new PaymentBuilder()
                .WithAmount(1234)
            )
            .Build();
        var dto = new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(0);
        dto.TotalPaymentAmount.ShouldBe(1534);
        dto.TotalBalance.ShouldBe(-1534);
    }

    [Test]
    public void WithExpensesAndPayments_ShouldBalance()
    {
        var group = new GroupBuilder()
            .AddExpense(new ExpenseBuilder()
                .WithAmount(100)
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(200)
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(1234)
            )
            .AddPayment(new PaymentBuilder()
                .WithAmount(100)
            )
            .AddPayment(new PaymentBuilder()
                .WithAmount(200)
            )
            .AddPayment(new PaymentBuilder()
                .WithAmount(1234)
            )
            .Build();
        var dto = new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(1534);
        dto.TotalPaymentAmount.ShouldBe(1534);
        dto.TotalBalance.ShouldBe(0);
    }
}