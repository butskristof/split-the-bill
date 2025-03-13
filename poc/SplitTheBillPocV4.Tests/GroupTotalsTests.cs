using Shouldly;
using SplitTheBillPocV4.Models;
using SplitTheBillPocV4.Modules;
using SplitTheBillPocV4.Tests.TestData;
using SplitTheBillPocV4.Tests.TestData.Builders;

namespace SplitTheBillPocV4.Tests;

internal sealed class GroupTotalsTests
{
    [Test]
    public void EmptyGroup_TotalAmountsAreZero()
    {
        var group = new GroupBuilder()
            .Build();
        var model = new EnrichedGroupModel(group);

        model.TotalExpenseAmount.ShouldBe(0);
        model.TotalPaymentAmount.ShouldBe(0);
        model.TotalAmountDue.ShouldBe(0);
    }

    [Test]
    public void WithExpenses_Totals()
    {
        var group = new GroupBuilder()
            .AddExpense(new ExpenseBuilder()
                .WithAmount(500m)
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(100m)
            )
            .Build();
        var model = new EnrichedGroupModel(group);

        model.TotalExpenseAmount.ShouldBe(600m);
        model.TotalPaymentAmount.ShouldBe(0);
        model.TotalAmountDue.ShouldBe(600m);
    }

    [Test]
    public void WithPayments_Totals()
    {
        var group = new GroupBuilder()
            .AddExpense(new ExpenseBuilder()
                .WithAmount(500m)
            )
            .AddPayment(new PaymentBuilder()
                .WithAmount(100m)
            )
            .Build();
        var model = new EnrichedGroupModel(group);
        
        model.TotalExpenseAmount.ShouldBe(500m);
        model.TotalPaymentAmount.ShouldBe(100m);
        model.TotalAmountDue.ShouldBe(400m);
    }

    [Test]
    public void EvenlySplit_Balances()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(1500m)
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .Build();
        var model = new EnrichedGroupModel(group);
        
        model.TotalExpenseAmount.ShouldBe(1500m);
        model.TotalPaymentAmount.ShouldBe(0);
        model.TotalAmountDue.ShouldBe(1500m);
    }

}