using Shouldly;
using SplitTheBillPocV4.Models;
using SplitTheBillPocV4.Modules;
using SplitTheBillPocV4.Tests.TestData;
using SplitTheBillPocV4.Tests.TestData.Builders;

namespace SplitTheBillPocV4.Tests;

internal sealed class PocV4Tests
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

    [Test]
    public void SplitTypeEvenly_TotalAmountPerMember1()
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
    public void SplitTypeEvenly_TotalAmountPerMember2()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity()])
                .WithAmount(2000m)
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .AddExpense(new ExpenseBuilder()
                .WithParticipants([Members.Bob.Entity(), Members.Charlie.Entity()])
                .WithAmount(1000m)
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .AddExpense(new ExpenseBuilder()
                .WithParticipants([Members.Bob.Entity(), Members.Charlie.Entity()])
                .WithAmount(1000m)
                .WithPaidByMemberId(Members.Charlie.Id)
            )
            .Build();
        var model = new EnrichedGroupModel(group);

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
    
    // [Test]
    // public void PaidByMember_HasNegativeAmountDueBalance()
    // {
    //     var group = new GroupBuilder()
    //         .WithMembers([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
    //         .AddExpense(new ExpenseBuilder()
    //             .WithAmount(1500m)
    //             .WithParticipants([
    //                 new ExpenseParticipantBuilder()
    //                     .WithMemberId(Members.Alice.Id),
    //                 new ExpenseParticipantBuilder()
    //                     .WithMemberId(Members.Bob.Id),
    //                 new ExpenseParticipantBuilder()
    //                     .WithMemberId(Members.Charlie.Id),
    //             ])
    //             .WithPaidByMemberId(Members.Alice.Id)
    //         )
    //         // .AddPayment(new PaymentBuilder()
    //         //     .WithAmount(1500m)
    //         //     .WithMemberId(Members.Alice.Id))
    //         .Build();
    //     var model = new EnrichedGroupModel(group);
    //
    //     // dto.TotalExpenseAmount.ShouldBe(1500m);
    //     // // dto.TotalPaymentAmount.ShouldBe(1500m);
    //     // // dto.AmountDue.ShouldBe(0);
    //     // dto.AmountsDueByMember[Members.Alice.Id].ShouldBe(-1000m);
    //     // dto.AmountsDueByMember[Members.Bob.Id].ShouldBe(500m);
    //     // dto.AmountsDueByMember[Members.Charlie.Id].ShouldBe(500m);
    // }
}