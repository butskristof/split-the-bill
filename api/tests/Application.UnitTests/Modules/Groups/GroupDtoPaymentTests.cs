using Shouldly;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.UnitTests.TestData;
using SplitTheBill.Application.UnitTests.TestData.Builders;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.UnitTests.Modules.Groups;

internal sealed class GroupDtoPaymentTests
{
    #region BasicPaymentTests

    [Test]
    public void PaymentReceived_TotalPaymentReceivedAmount()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddPayment(new PaymentBuilder()
                .WithAmount(100m)
                .WithSendingMemberId(Members.Bob.Id)
                .WithReceivingMemberId(Members.Alice.Id)
            )
            .Build();
        var dto = new GroupDto(group);

        dto.TotalPaymentAmount.ShouldBe(100);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalPaymentReceivedAmount.ShouldBe(100m);
        alice.TotalPaymentSentAmount.ShouldBe(0m);
        alice.TotalBalance.ShouldBe(-100m);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalPaymentReceivedAmount.ShouldBe(0m);
        bob.TotalPaymentSentAmount.ShouldBe(100m);
        bob.TotalBalance.ShouldBe(100m);
    }

    [Test]
    public void PaymentSent_TotalPaymentSentAmount()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddPayment(new PaymentBuilder()
                .WithAmount(100m)
                .WithSendingMemberId(Members.Alice.Id)
                .WithReceivingMemberId(Members.Bob.Id)
            )
            .Build();
        var dto = new GroupDto(group);
        
        dto.TotalPaymentAmount.ShouldBe(100);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalPaymentReceivedAmount.ShouldBe(0m);
        alice.TotalPaymentSentAmount.ShouldBe(100m);
        alice.TotalBalance.ShouldBe(100m);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalPaymentReceivedAmount.ShouldBe(100m);
        bob.TotalPaymentSentAmount.ShouldBe(0m);
        bob.TotalBalance.ShouldBe(-100m);
    }

    [Test]
    public void MultiplePayments_TotalPaymentAmounts()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
            .AddPayment(new PaymentBuilder()
                .WithAmount(100m)
                .WithSendingMemberId(Members.Alice.Id)
                .WithReceivingMemberId(Members.Bob.Id)
            )
            .AddPayment(new PaymentBuilder()
                .WithAmount(50m)
                .WithSendingMemberId(Members.Bob.Id)
                .WithReceivingMemberId(Members.Charlie.Id)
            )
            .AddPayment(new PaymentBuilder()
                .WithAmount(25m)
                .WithSendingMemberId(Members.Charlie.Id)
                .WithReceivingMemberId(Members.Alice.Id)
            )
            .Build();
        var dto = new GroupDto(group);

        dto.TotalPaymentAmount.ShouldBe(175m);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalPaymentReceivedAmount.ShouldBe(25m);
        alice.TotalPaymentSentAmount.ShouldBe(100m);
        alice.TotalBalance.ShouldBe(75m);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalPaymentReceivedAmount.ShouldBe(100m);
        bob.TotalPaymentSentAmount.ShouldBe(50m);
        bob.TotalBalance.ShouldBe(-50m);

        var charlie = dto.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalPaymentReceivedAmount.ShouldBe(50m);
        charlie.TotalPaymentSentAmount.ShouldBe(25m);
        charlie.TotalBalance.ShouldBe(-25m);
    }

    #endregion

    #region CombinedExpenseAndPaymentTests

    [Test]
    public void SimpleExpenseWithPayment_TotalAmounts()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(100m)
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity()])
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .AddPayment(new PaymentBuilder()
                .WithAmount(50m)
                .WithSendingMemberId(Members.Bob.Id)
                .WithReceivingMemberId(Members.Alice.Id)
            )
            .Build();
        var dto = new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(100m);
        dto.TotalPaymentAmount.ShouldBe(50m);
        dto.TotalBalance.ShouldBe(50m);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(50m);
        alice.TotalExpensePaidAmount.ShouldBe(100m);
        alice.TotalPaymentReceivedAmount.ShouldBe(50m);
        alice.TotalAmountOwed.ShouldBe(0m);
        alice.TotalBalance.ShouldBe(0m);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(50m);
        bob.TotalExpensePaidAmount.ShouldBe(0m);
        bob.TotalPaymentSentAmount.ShouldBe(50m);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(0m);
        bob.TotalBalance.ShouldBe(0m);
    }

    [Test]
    public void PartialPaymentOfDebt_TotalAmounts()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(100m)
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity()])
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .AddPayment(new PaymentBuilder()
                .WithAmount(25m)
                .WithSendingMemberId(Members.Bob.Id)
                .WithReceivingMemberId(Members.Alice.Id)
            )
            .Build();
        var dto = new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(100m);
        dto.TotalPaymentAmount.ShouldBe(25m);
        dto.TotalBalance.ShouldBe(75m);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(50m);
        alice.TotalExpensePaidAmount.ShouldBe(100m);
        alice.TotalPaymentReceivedAmount.ShouldBe(25m);
        alice.TotalAmountOwed.ShouldBe(25m);
        alice.TotalBalance.ShouldBe(25m);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(50m);
        bob.TotalExpensePaidAmount.ShouldBe(0m);
        bob.TotalPaymentSentAmount.ShouldBe(25m);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(25m);
        bob.TotalBalance.ShouldBe(-25m);
    }

    [Test]
    public void OverpaymentOfDebt_TotalAmounts()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(100m)
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity()])
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .AddPayment(new PaymentBuilder()
                .WithAmount(75m)
                .WithSendingMemberId(Members.Bob.Id)
                .WithReceivingMemberId(Members.Alice.Id)
            )
            .Build();
        var dto = new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(100m);
        dto.TotalPaymentAmount.ShouldBe(75m);
        dto.TotalBalance.ShouldBe(25m);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(50m);
        alice.TotalExpensePaidAmount.ShouldBe(100m);
        alice.TotalPaymentReceivedAmount.ShouldBe(75m);
        alice.TotalAmountOwed.ShouldBe(-25m); // Negative because Bob overpaid
        alice.TotalBalance.ShouldBe(-25m);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(50m);
        bob.TotalExpensePaidAmount.ShouldBe(0m);
        bob.TotalPaymentSentAmount.ShouldBe(75m);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(-25m); // Negative because Bob overpaid
        bob.TotalBalance.ShouldBe(25m);
    }

    #endregion

    #region ComplexScenarios

    [Test]
    public void ComplexScenarioWithThreeMembers_TotalAmounts()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
            // Expense 1: Alice pays, split evenly
            .AddExpense(new ExpenseBuilder()
                .WithAmount(300m)
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
                .WithPaidByMemberId(Members.Alice.Id)
            )
            // Expense 2: Bob pays, split percentually
            .AddExpense(new ExpenseBuilder()
                .WithAmount(400m)
                .WithSplitType(ExpenseSplitType.Percentual)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithPercentualShare(25)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithPercentualShare(25)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Charlie.Id)
                    .WithPercentualShare(50)
                )
                .WithPaidByMemberId(Members.Bob.Id)
            )
            // Expense 3: Charlie pays, split exact amounts
            .AddExpense(new ExpenseBuilder()
                .WithAmount(500m)
                .WithSplitType(ExpenseSplitType.ExactAmount)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithExactShare(150)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithExactShare(150)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Charlie.Id)
                    .WithExactShare(200)
                )
                .WithPaidByMemberId(Members.Charlie.Id)
            )
            // Payment 1: Bob pays Alice
            .AddPayment(new PaymentBuilder()
                .WithAmount(100m)
                .WithSendingMemberId(Members.Bob.Id)
                .WithReceivingMemberId(Members.Alice.Id)
            )
            // Payment 2: Charlie pays Alice
            .AddPayment(new PaymentBuilder()
                .WithAmount(150m)
                .WithSendingMemberId(Members.Charlie.Id)
                .WithReceivingMemberId(Members.Alice.Id)
            )
            // Payment 3: Charlie pays Bob
            .AddPayment(new PaymentBuilder()
                .WithAmount(200m)
                .WithSendingMemberId(Members.Charlie.Id)
                .WithReceivingMemberId(Members.Bob.Id)
            )
            .Build();
        var dto = new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(1200m);
        dto.TotalPaymentAmount.ShouldBe(450m);
        dto.TotalBalance.ShouldBe(750m);

        // Verify Alice's calculations
        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(350m);
        alice.TotalExpensePaidAmount.ShouldBe(300m);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(250m);
        alice.TotalPaymentReceivedAmount.ShouldBe(250m);
        alice.TotalPaymentSentAmount.ShouldBe(0m);
        alice.TotalAmountOwed.ShouldBe(-50m);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(250m);
        alice.TotalBalance.ShouldBe(-300m);

        // Verify Bob's calculations
        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(350m);
        bob.TotalExpensePaidAmount.ShouldBe(400m);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(250m);
        bob.TotalPaymentReceivedAmount.ShouldBe(200m);
        bob.TotalPaymentSentAmount.ShouldBe(100m);
        bob.TotalAmountOwed.ShouldBe(100m);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(150m);
        bob.TotalBalance.ShouldBe(-50m);

        // Verify Charlie's calculations
        var charlie = dto.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(500m);
        charlie.TotalExpensePaidAmount.ShouldBe(500m);
        charlie.TotalExpenseAmountPaidByOtherMembers.ShouldBe(300m);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0m);
        charlie.TotalPaymentSentAmount.ShouldBe(350m);
        charlie.TotalAmountOwed.ShouldBe(300m);
        charlie.TotalAmountOwedToOtherMembers.ShouldBe(-50m);
        charlie.TotalBalance.ShouldBe(350m);
    }

    #endregion

    #region EdgeCases

    [Test]
    public void ZeroAmountPayment_ShouldNotAffectTotals()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(100m)
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity()])
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .AddPayment(new PaymentBuilder()
                .WithAmount(0m)
                .WithSendingMemberId(Members.Bob.Id)
                .WithReceivingMemberId(Members.Alice.Id)
            )
            .Build();
        var dto = new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(100m);
        dto.TotalPaymentAmount.ShouldBe(0m);
        dto.TotalBalance.ShouldBe(100m);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalPaymentReceivedAmount.ShouldBe(0m);
        alice.TotalBalance.ShouldBe(50m);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalPaymentSentAmount.ShouldBe(0m);
        bob.TotalBalance.ShouldBe(-50m);
    }

    #endregion
}