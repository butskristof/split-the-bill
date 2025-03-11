using Shouldly;
using SplitTheBillPocV4.Models;
using SplitTheBillPocV4.Modules;
using SplitTheBillPocV4.Tests.TestData;
using SplitTheBillPocV4.Tests.TestData.Builders;

namespace SplitTheBillPocV4.Tests;

internal sealed class MemberTotalTests_SplitPercentually
{
    [Test]
    public void SplitPercentually_TotalAmountsPerMember1()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(1000m)
                .WithSplitType(ExpenseSplitType.Percentual)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithPercentualSplitShare(50)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithPercentualSplitShare(50)
                )
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .Build();
        var model = new EnrichedGroupModel(group);

        model.TotalExpenseAmount.ShouldBe(1000);
        model.TotalPaymentAmount.ShouldBe(0);
        model.TotalAmountDue.ShouldBe(1000);

        var alice = model.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(500);
        alice.TotalExpensePaidAmount.ShouldBe(1000);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(500);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(0);
        alice.Balance.ShouldBe(500);

        var bob = model.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(500);
        bob.TotalExpensePaidAmount.ShouldBe(0);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(500);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(0);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(500);
        bob.Balance.ShouldBe(-500);
    }

    [Test]
    public void SplitPercentually_TotalAmountsPerMember2()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(1000m)
                .WithSplitType(ExpenseSplitType.Percentual)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithPercentualSplitShare(10)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithPercentualSplitShare(90)
                )
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .Build();
        var model = new EnrichedGroupModel(group);

        model.TotalExpenseAmount.ShouldBe(1000);
        model.TotalPaymentAmount.ShouldBe(0);
        model.TotalAmountDue.ShouldBe(1000);

        var alice = model.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(100);
        alice.TotalExpensePaidAmount.ShouldBe(1000);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(900);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(0);
        alice.Balance.ShouldBe(900);

        var bob = model.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(900);
        bob.TotalExpensePaidAmount.ShouldBe(0);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(900);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(0);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(900);
        bob.Balance.ShouldBe(-900);
    }

    [Test]
    public void SplitPercentually_TotalAmountsPerMember3()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(1000m)
                .WithSplitType(ExpenseSplitType.Percentual)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithPercentualSplitShare(10)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithPercentualSplitShare(90)
                )
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .Build();
        var model = new EnrichedGroupModel(group);

        model.TotalExpenseAmount.ShouldBe(1000);
        model.TotalPaymentAmount.ShouldBe(0);
        model.TotalAmountDue.ShouldBe(1000);

        var alice = model.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(100);
        alice.TotalExpensePaidAmount.ShouldBe(1000);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(900);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(0);
        alice.Balance.ShouldBe(900);

        var bob = model.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(900);
        bob.TotalExpensePaidAmount.ShouldBe(0);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(900);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(0);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(900);
        bob.Balance.ShouldBe(-900);

        var charlie = model.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(0);
        charlie.TotalExpensePaidAmount.ShouldBe(0);
        charlie.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        charlie.TotalAmountOwed.ShouldBe(0);
        charlie.TotalAmountOwedToOtherMembers.ShouldBe(0);
        charlie.Balance.ShouldBe(0);
    }

    [Test]
    public void SplitPercentually_TotalAmountsPerMember4()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(1000m)
                .WithSplitType(ExpenseSplitType.Percentual)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithPercentualSplitShare(10)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithPercentualSplitShare(90)
                )
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(123)
                .WithSplitType(ExpenseSplitType.Percentual)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithPercentualSplitShare(60)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithPercentualSplitShare(30)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Charlie.Id)
                    .WithPercentualSplitShare(10)
                )
                .WithPaidByMemberId(Members.Bob.Id)
            )
            .Build();
        var model = new EnrichedGroupModel(group);

        model.TotalExpenseAmount.ShouldBe(1123);
        model.TotalPaymentAmount.ShouldBe(0);
        model.TotalAmountDue.ShouldBe(1123);

        var alice = model.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(173.8m);
        alice.TotalExpensePaidAmount.ShouldBe(1000);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(73.8m);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(900);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(73.8m);
        alice.Balance.ShouldBe(826.2m);

        var bob = model.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(936.9m);
        bob.TotalExpensePaidAmount.ShouldBe(123);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(900);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(86.1m);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(900);
        bob.Balance.ShouldBe(-813.9m);

        var charlie = model.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(12.3m);
        charlie.TotalExpensePaidAmount.ShouldBe(0);
        charlie.TotalExpenseAmountPaidByOtherMembers.ShouldBe(12.3m);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        charlie.TotalAmountOwed.ShouldBe(0);
        charlie.TotalAmountOwedToOtherMembers.ShouldBe(12.3m);
        charlie.Balance.ShouldBe(-12.3m);
    }

    [Test]
    public void SplitPercentually_TotalAmountsPerMember5()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithSplitType(ExpenseSplitType.Percentual)
                .WithAmount(100m)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithPercentualSplitShare(100)
                )
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .Build();
        var model = new EnrichedGroupModel(group);

        model.TotalExpenseAmount.ShouldBe(100);
        model.TotalPaymentAmount.ShouldBe(0);
        model.TotalAmountDue.ShouldBe(100);

        var alice = model.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(100);
        alice.TotalExpensePaidAmount.ShouldBe(100);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(0);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(0);
        alice.Balance.ShouldBe(0);
        
        var bob = model.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(0);
        bob.TotalExpensePaidAmount.ShouldBe(0);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(0);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(0);
        bob.Balance.ShouldBe(0);
    }

    [Test]
    public void SplitPercentually_TotalAmountsPerMember6()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithSplitType(ExpenseSplitType.Percentual)
                .WithAmount(100m)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithPercentualSplitShare(100)
                )
                .WithPaidByMemberId(Members.Bob.Id)
            )
            .Build();
        var model = new EnrichedGroupModel(group);

        model.TotalExpenseAmount.ShouldBe(100);
        model.TotalPaymentAmount.ShouldBe(0);
        model.TotalAmountDue.ShouldBe(100);

        var alice = model.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(100);
        alice.TotalExpensePaidAmount.ShouldBe(0);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(100);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(0);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(100);
        alice.Balance.ShouldBe(-100);
        
        var bob = model.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(0);
        bob.TotalExpensePaidAmount.ShouldBe(100);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(100);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(0);
        bob.Balance.ShouldBe(100);
    }
}