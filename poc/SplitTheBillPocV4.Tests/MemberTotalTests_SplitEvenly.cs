using Shouldly;
using SplitTheBillPocV4.Models;
using SplitTheBillPocV4.Modules;
using SplitTheBillPocV4.Tests.TestData;
using SplitTheBillPocV4.Tests.TestData.Builders;

namespace SplitTheBillPocV4.Tests;

internal sealed class MemberTotalTests_SplitEvenly
{
    [Test]
    public void SplitTypeEvenly_TotalAmountsPerMember1()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(1500m)
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .Build();
        var model = new EnrichedGroupModel(group);
        
        model.TotalExpenseAmount.ShouldBe(1500);
        model.TotalPaymentAmount.ShouldBe(0);
        model.TotalAmountDue.ShouldBe(1500);

        var alice = model.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(500m);
        alice.TotalExpensePaidAmount.ShouldBe(1500m);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0m);
        alice.TotalPaymentReceivedAmount.ShouldBe(0m);
        alice.TotalPaymentSentAmount.ShouldBe(0m);
        alice.TotalAmountOwed.ShouldBe(1000m);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(0m);
        alice.Balance.ShouldBe(1000m);
        
        var bob = model.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(500m);
        bob.TotalExpensePaidAmount.ShouldBe(0m);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(500m);
        bob.TotalPaymentReceivedAmount.ShouldBe(0m);
        bob.TotalPaymentSentAmount.ShouldBe(0m);
        bob.TotalAmountOwed.ShouldBe(0m);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(500m);
        bob.Balance.ShouldBe(-500m);
        
        var charlie = model.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(500m);
        charlie.TotalExpensePaidAmount.ShouldBe(0m);
        charlie.TotalExpenseAmountPaidByOtherMembers.ShouldBe(500m);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0m);
        charlie.TotalPaymentSentAmount.ShouldBe(0m);
        charlie.TotalAmountOwed.ShouldBe(0m);
        charlie.TotalAmountOwedToOtherMembers.ShouldBe(500m);
        charlie.Balance.ShouldBe(-500m);
    }

    [Test]
    public void SplitTypeEvenly_TotalAmountsPerMember2()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity()])
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithAmount(2000m)
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .AddExpense(new ExpenseBuilder()
                .WithParticipants([Members.Bob.Entity(), Members.Charlie.Entity()])
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithAmount(1000m)
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .AddExpense(new ExpenseBuilder()
                .WithParticipants([Members.Bob.Entity(), Members.Charlie.Entity()])
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithAmount(1000m)
                .WithPaidByMemberId(Members.Charlie.Id)
            )
            .Build();
        var model = new EnrichedGroupModel(group);
        
        model.TotalExpenseAmount.ShouldBe(4000);
        model.TotalPaymentAmount.ShouldBe(0);
        model.TotalAmountDue.ShouldBe(4000);

        var alice = model.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(1000);
        alice.TotalExpensePaidAmount.ShouldBe(3000);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(2000);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(0);
        alice.Balance.ShouldBe(2000);
        
        var bob = model.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(2000);
        bob.TotalExpensePaidAmount.ShouldBe(0);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(2000);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(0);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(2000);
        bob.Balance.ShouldBe(-2000);
        
        var charlie = model.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(1000);
        charlie.TotalExpensePaidAmount.ShouldBe(1000);
        charlie.TotalExpenseAmountPaidByOtherMembers.ShouldBe(500);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        charlie.TotalAmountOwed.ShouldBe(500);
        charlie.TotalAmountOwedToOtherMembers.ShouldBe(500);
        charlie.Balance.ShouldBe(0);
    }

    [Test]
    public void SplitTypeEvenly_TotalAmountsPerMember3()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(900m)
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(400m)
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithParticipants([Members.Bob.Entity(), Members.Charlie.Entity()])
                .WithPaidByMemberId(Members.Bob.Id)
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(600m)
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
                .WithPaidByMemberId(Members.Charlie.Id)
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(300m)
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity()])
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .Build();
        var model = new EnrichedGroupModel(group);
        
        model.TotalExpenseAmount.ShouldBe(2200);
        model.TotalPaymentAmount.ShouldBe(0);
        model.TotalAmountDue.ShouldBe(2200);
        
        var alice = model.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(650);
        alice.TotalExpensePaidAmount.ShouldBe(1200);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(200);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(750);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(200);
        alice.Balance.ShouldBe(550);
        
        var bob = model.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(850);
        bob.TotalExpensePaidAmount.ShouldBe(400);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(650);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(200);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(650);
        bob.Balance.ShouldBe(-450);
        
        var charlie = model.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(700);
        charlie.TotalExpensePaidAmount.ShouldBe(600);
        charlie.TotalExpenseAmountPaidByOtherMembers.ShouldBe(500);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        charlie.TotalAmountOwed.ShouldBe(400);
        charlie.TotalAmountOwedToOtherMembers.ShouldBe(500);
        charlie.Balance.ShouldBe(-100);
    }

    [Test]
    public void SplitTypeEvenly_TotalAmountsPerMember4_DifficultNumbers()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(1000m)
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(2000m)
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
                .WithPaidByMemberId(Members.Charlie.Id)
            )
            .Build();
        var model = new EnrichedGroupModel(group);
        
        model.TotalExpenseAmount.ShouldBe(3000);
        model.TotalPaymentAmount.ShouldBe(0);
        model.TotalAmountDue.ShouldBe(3000);
        
        var alice = model.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(1000);
        alice.TotalExpensePaidAmount.ShouldBe(1000);
        Math.Round(alice.TotalExpenseAmountPaidByOtherMembers, 2).ShouldBe(666.67m);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        Math.Round(alice.TotalAmountOwed, 2).ShouldBe(666.67m);
        Math.Round(alice.TotalAmountOwedToOtherMembers, 2).ShouldBe(666.67m);
        alice.Balance.ShouldBe(0);
        
        var bob = model.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(1000);
        bob.TotalExpensePaidAmount.ShouldBe(0);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(1000);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(0);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(1000);
        bob.Balance.ShouldBe(-1000);
        
        var charlie = model.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(1000);
        charlie.TotalExpensePaidAmount.ShouldBe(2000);
        Math.Round(charlie.TotalExpenseAmountPaidByOtherMembers, 2).ShouldBe(333.33m);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        Math.Round(charlie.TotalAmountOwed, 2).ShouldBe(1333.33m);
        Math.Round(charlie.TotalAmountOwedToOtherMembers, 2).ShouldBe(333.33m);
        charlie.Balance.ShouldBe(1000m);
    }
}