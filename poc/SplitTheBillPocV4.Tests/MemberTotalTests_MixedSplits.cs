using Shouldly;
using SplitTheBillPocV4.Models;
using SplitTheBillPocV4.Modules;
using SplitTheBillPocV4.Tests.TestData;
using SplitTheBillPocV4.Tests.TestData.Builders;

namespace SplitTheBillPocV4.Tests;

internal sealed class MemberTotalTests_MixedSplits
{
    [Test]
    public void MixedSplits_TotalAmountsPerMember1()
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
                .WithAmount(900m)
                .WithSplitType(ExpenseSplitType.Percentual)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithPercentualSplitShare(20)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithPercentualSplitShare(80)
                )
                .WithPaidByMemberId(Members.Bob.Id)
            )
            .Build();
        var model = new EnrichedGroupModel(group);
        
        model.TotalExpenseAmount.ShouldBe(1900);
        model.TotalPaymentAmount.ShouldBe(0);
        model.TotalAmountDue.ShouldBe(1900);
        
        var alice = model.Members.Single(m => m.Id == Members.Alice.Id);
        Math.Round(alice.TotalExpenseAmount, 2).ShouldBe(513.33m);
        alice.TotalExpensePaidAmount.ShouldBe(1000);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(180);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        Math.Round(alice.TotalAmountOwed, 2).ShouldBe(666.67m);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(180);
        Math.Round(alice.Balance, 2).ShouldBe(486.67m);
        
        var bob = model.Members.Single(m => m.Id == Members.Bob.Id);
        Math.Round(bob.TotalExpenseAmount, 2).ShouldBe(1053.33m);
        bob.TotalExpensePaidAmount.ShouldBe(900);
        Math.Round(bob.TotalExpenseAmountPaidByOtherMembers, 2).ShouldBe(333.33m);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(180);
        Math.Round(bob.TotalAmountOwedToOtherMembers, 2).ShouldBe(333.33m);
        Math.Round(bob.Balance, 2).ShouldBe(-153.33m);
        
        var charlie = model.Members.Single(m => m.Id == Members.Charlie.Id);
        Math.Round(charlie.TotalExpenseAmount, 2).ShouldBe(333.33m);
        charlie.TotalExpensePaidAmount.ShouldBe(0);
        Math.Round(charlie.TotalExpenseAmountPaidByOtherMembers, 2).ShouldBe(333.33m);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        charlie.TotalAmountOwed.ShouldBe(0);
        Math.Round(charlie.TotalAmountOwedToOtherMembers, 2).ShouldBe(333.33m);
        Math.Round(charlie.Balance, 2).ShouldBe(-333.33m);
    }
    
    [Test]
    public void MixedSplits_TotalAmountsPerMember2()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(600m)
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(900m)
                .WithSplitType(ExpenseSplitType.ExactAmount)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithExactAmountSplitShare(300)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithExactAmountSplitShare(400)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Charlie.Id)
                    .WithExactAmountSplitShare(200)
                )
                .WithPaidByMemberId(Members.Charlie.Id)
            )
            .Build();
        var model = new EnrichedGroupModel(group);
        
        model.TotalExpenseAmount.ShouldBe(1500);
        model.TotalPaymentAmount.ShouldBe(0);
        model.TotalAmountDue.ShouldBe(1500);
        
        var alice = model.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(500);
        alice.TotalExpensePaidAmount.ShouldBe(600);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(300);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(400);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(300);
        alice.Balance.ShouldBe(100);
        
        var bob = model.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(600);
        bob.TotalExpensePaidAmount.ShouldBe(0);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(600);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(0);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(600);
        bob.Balance.ShouldBe(-600);
        
        var charlie = model.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(400);
        charlie.TotalExpensePaidAmount.ShouldBe(900);
        charlie.TotalExpenseAmountPaidByOtherMembers.ShouldBe(200);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        charlie.TotalAmountOwed.ShouldBe(700);
        charlie.TotalAmountOwedToOtherMembers.ShouldBe(200);
        charlie.Balance.ShouldBe(500);
    }
    
    [Test]
    public void MixedSplits_TotalAmountsPerMember3()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(300m)
                .WithSplitType(ExpenseSplitType.Percentual)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithPercentualSplitShare(50)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithPercentualSplitShare(30)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Charlie.Id)
                    .WithPercentualSplitShare(20)
                )
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(500m)
                .WithSplitType(ExpenseSplitType.ExactAmount)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithExactAmountSplitShare(200)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithExactAmountSplitShare(300)
                )
                .WithPaidByMemberId(Members.Bob.Id)
            )
            .Build();
        var model = new EnrichedGroupModel(group);
        
        model.TotalExpenseAmount.ShouldBe(800);
        model.TotalPaymentAmount.ShouldBe(0);
        model.TotalAmountDue.ShouldBe(800);
        
        var alice = model.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(350);
        alice.TotalExpensePaidAmount.ShouldBe(300);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(200);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(150);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(200);
        alice.Balance.ShouldBe(-50);
        
        var bob = model.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(390);
        bob.TotalExpensePaidAmount.ShouldBe(500);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(90);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(200);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(90);
        bob.Balance.ShouldBe(110);
        
        var charlie = model.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(60);
        charlie.TotalExpensePaidAmount.ShouldBe(0);
        charlie.TotalExpenseAmountPaidByOtherMembers.ShouldBe(60);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        charlie.TotalAmountOwed.ShouldBe(0);
        charlie.TotalAmountOwedToOtherMembers.ShouldBe(60);
        charlie.Balance.ShouldBe(-60);
    }
    
    [Test]
    public void MixedSplits_TotalAmountsPerMember4_AllThreeSplitTypes()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(300m)
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(400m)
                .WithSplitType(ExpenseSplitType.Percentual)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithPercentualSplitShare(25)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithPercentualSplitShare(25)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Charlie.Id)
                    .WithPercentualSplitShare(50)
                )
                .WithPaidByMemberId(Members.Bob.Id)
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(500m)
                .WithSplitType(ExpenseSplitType.ExactAmount)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithExactAmountSplitShare(150)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithExactAmountSplitShare(150)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Charlie.Id)
                    .WithExactAmountSplitShare(200)
                )
                .WithPaidByMemberId(Members.Charlie.Id)
            )
            .Build();
        var model = new EnrichedGroupModel(group);
        
        model.TotalExpenseAmount.ShouldBe(1200);
        model.TotalPaymentAmount.ShouldBe(0);
        model.TotalAmountDue.ShouldBe(1200);
        
        var alice = model.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(350);
        alice.TotalExpensePaidAmount.ShouldBe(300);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(250);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(200);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(250);
        alice.Balance.ShouldBe(-50);
        
        var bob = model.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(350);
        bob.TotalExpensePaidAmount.ShouldBe(400);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(250);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(300);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(250);
        bob.Balance.ShouldBe(50);
        
        var charlie = model.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(500);
        charlie.TotalExpensePaidAmount.ShouldBe(500);
        charlie.TotalExpenseAmountPaidByOtherMembers.ShouldBe(300);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        charlie.TotalAmountOwed.ShouldBe(300);
        charlie.TotalAmountOwedToOtherMembers.ShouldBe(300);
        charlie.Balance.ShouldBe(0);
    }
    
    [Test]
    public void MixedSplits_TotalAmountsPerMember5_ComplexScenario()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(1000m)
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity()])
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(600m)
                .WithSplitType(ExpenseSplitType.Percentual)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithPercentualSplitShare(30)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithPercentualSplitShare(20)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Charlie.Id)
                    .WithPercentualSplitShare(50)
                )
                .WithPaidByMemberId(Members.Bob.Id)
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(750m)
                .WithSplitType(ExpenseSplitType.ExactAmount)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithExactAmountSplitShare(250)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Charlie.Id)
                    .WithExactAmountSplitShare(500)
                )
                .WithPaidByMemberId(Members.Charlie.Id)
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(450m)
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithParticipants([Members.Bob.Entity(), Members.Charlie.Entity()])
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .Build();
        var model = new EnrichedGroupModel(group);
        
        model.TotalExpenseAmount.ShouldBe(2800);
        model.TotalPaymentAmount.ShouldBe(0);
        model.TotalAmountDue.ShouldBe(2800);
        
        var alice = model.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(930);
        alice.TotalExpensePaidAmount.ShouldBe(1450);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(430);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(950);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(430);
        alice.Balance.ShouldBe(520);
        
        var bob = model.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(845);
        bob.TotalExpensePaidAmount.ShouldBe(600);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(725);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(480);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(725);
        bob.Balance.ShouldBe(-245);
        
        var charlie = model.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(1025);
        charlie.TotalExpensePaidAmount.ShouldBe(750);
        charlie.TotalExpenseAmountPaidByOtherMembers.ShouldBe(525);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        charlie.TotalAmountOwed.ShouldBe(250);
        charlie.TotalAmountOwedToOtherMembers.ShouldBe(525);
        charlie.Balance.ShouldBe(-275);
    }
}