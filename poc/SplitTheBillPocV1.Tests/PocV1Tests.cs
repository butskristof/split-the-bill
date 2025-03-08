using Shouldly;
using SplitTheBillPoc.Tests.TestData;
using SplitTheBillPoc.Tests.TestData.Builders;
using SplitTheBillPocV1.Modules.Groups;

namespace SplitTheBillPoc.Tests;

public class PocV1Tests
{
    [Test]
    public void EmptyGroup_ZeroAmounts()
    {
        var group = new GroupBuilder().Build();
        var dto = group.MapToDetailedGroupDTO();
        dto.TotalExpenseAmount.ShouldBe(0);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.AmountDue.ShouldBe(0);
        dto.ExpenseAmountPerMember.ShouldBe(0);
        dto.AmountsDueByMember.Values.ShouldAllBe(v => v == 0);
    }

    [Test]
    public void WithMembers_NoExpenses_NoPayments_ZeroAmounts()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .Build();
        var dto = group.MapToDetailedGroupDTO();

        dto.TotalExpenseAmount.ShouldBe(0);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.AmountDue.ShouldBe(0);
        dto.ExpenseAmountPerMember.ShouldBe(0);
        dto.AmountsDueByMember.Values.ShouldAllBe(v => v == 0);
    }

    [Test]
    public void WithMembers_WithExpenses_NoPayments()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(100m)
                .Build())
            .AddExpense(new ExpenseBuilder()
                .WithAmount(200m)
                .Build())
            .Build();
        var dto = group.MapToDetailedGroupDTO();

        dto.TotalExpenseAmount.ShouldBe(300m);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.AmountDue.ShouldBe(300m);
        dto.ExpenseAmountPerMember.ShouldBe(150m);
        dto.AmountsDueByMember.Values.ShouldAllBe(v => v == 150m);
    }

    [Test]
    public void WithMembers_WithExpenses_WithPayments()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(100m)
                .Build())
            .AddExpense(new ExpenseBuilder()
                .WithAmount(200m)
                .Build())
            .AddPayment(new PaymentBuilder()
                .WithAmount(100m)
                .WithMemberId(Members.Alice.Id)
                .Build())
            .AddPayment(new PaymentBuilder()
                .WithAmount(50m)
                .WithMemberId(Members.Bob.Id)
                .Build())
            .Build();
        var dto = group.MapToDetailedGroupDTO();

        dto.TotalExpenseAmount.ShouldBe(300m);
        dto.TotalPaymentAmount.ShouldBe(150);
        dto.AmountDue.ShouldBe(150m);
        dto.ExpenseAmountPerMember.ShouldBe(150m);
        dto.AmountsDueByMember[Members.Alice.Id].ShouldBe(50m);
        dto.AmountsDueByMember[Members.Bob.Id].ShouldBe(100m);
    }
}