using Shouldly;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.UnitTests.TestData;
using SplitTheBill.Application.UnitTests.TestData.Builders;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.UnitTests.Modules.Groups;

internal sealed class GroupDtoMemberTotalTests
{
    #region ExpensesOnly

    #region SplitType_Evenly

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
        var dto =new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(1500);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(1500);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(500m);
        alice.TotalExpensePaidAmount.ShouldBe(1500m);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0m);
        alice.TotalPaymentReceivedAmount.ShouldBe(0m);
        alice.TotalPaymentSentAmount.ShouldBe(0m);
        alice.TotalAmountOwed.ShouldBe(1000m);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(0m);
        alice.TotalBalance.ShouldBe(1000m);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(500m);
        bob.TotalExpensePaidAmount.ShouldBe(0m);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(500m);
        bob.TotalPaymentReceivedAmount.ShouldBe(0m);
        bob.TotalPaymentSentAmount.ShouldBe(0m);
        bob.TotalAmountOwed.ShouldBe(0m);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(500m);
        bob.TotalBalance.ShouldBe(-500m);

        var charlie = dto.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(500m);
        charlie.TotalExpensePaidAmount.ShouldBe(0m);
        charlie.TotalExpenseAmountPaidByOtherMembers.ShouldBe(500m);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0m);
        charlie.TotalPaymentSentAmount.ShouldBe(0m);
        charlie.TotalAmountOwed.ShouldBe(0m);
        charlie.TotalAmountOwedToOtherMembers.ShouldBe(500m);
        charlie.TotalBalance.ShouldBe(-500m);
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
        var dto =new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(4000);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(4000);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(1000);
        alice.TotalExpensePaidAmount.ShouldBe(3000);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(2000);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(0);
        alice.TotalBalance.ShouldBe(2000);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(2000);
        bob.TotalExpensePaidAmount.ShouldBe(0);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(2000);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(0);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(2000);
        bob.TotalBalance.ShouldBe(-2000);

        var charlie = dto.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(1000);
        charlie.TotalExpensePaidAmount.ShouldBe(1000);
        charlie.TotalExpenseAmountPaidByOtherMembers.ShouldBe(500);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        charlie.TotalAmountOwed.ShouldBe(500);
        charlie.TotalAmountOwedToOtherMembers.ShouldBe(500);
        charlie.TotalBalance.ShouldBe(0);
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
        var dto =new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(2200);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(2200);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(650);
        alice.TotalExpensePaidAmount.ShouldBe(1200);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(200);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(750);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(200);
        alice.TotalBalance.ShouldBe(550);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(850);
        bob.TotalExpensePaidAmount.ShouldBe(400);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(650);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(200);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(650);
        bob.TotalBalance.ShouldBe(-450);

        var charlie = dto.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(700);
        charlie.TotalExpensePaidAmount.ShouldBe(600);
        charlie.TotalExpenseAmountPaidByOtherMembers.ShouldBe(500);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        charlie.TotalAmountOwed.ShouldBe(400);
        charlie.TotalAmountOwedToOtherMembers.ShouldBe(500);
        charlie.TotalBalance.ShouldBe(-100);
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
        var dto =new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(3000);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(3000);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(1000);
        alice.TotalExpensePaidAmount.ShouldBe(1000);
        Math.Round(alice.TotalExpenseAmountPaidByOtherMembers, 2).ShouldBe(666.67m);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        Math.Round(alice.TotalAmountOwed, 2).ShouldBe(666.67m);
        Math.Round(alice.TotalAmountOwedToOtherMembers, 2).ShouldBe(666.67m);
        alice.TotalBalance.ShouldBe(0);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(1000);
        bob.TotalExpensePaidAmount.ShouldBe(0);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(1000);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(0);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(1000);
        bob.TotalBalance.ShouldBe(-1000);

        var charlie = dto.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(1000);
        charlie.TotalExpensePaidAmount.ShouldBe(2000);
        Math.Round(charlie.TotalExpenseAmountPaidByOtherMembers, 2).ShouldBe(333.33m);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        Math.Round(charlie.TotalAmountOwed, 2).ShouldBe(1333.33m);
        Math.Round(charlie.TotalAmountOwedToOtherMembers, 2).ShouldBe(333.33m);
        charlie.TotalBalance.ShouldBe(1000m);
    }

    #endregion

    #region SplitType_Percentual

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
                    .WithPercentualShare(50)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithPercentualShare(50)
                )
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .Build();
        var dto =new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(1000);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(1000);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(500);
        alice.TotalExpensePaidAmount.ShouldBe(1000);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(500);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(0);
        alice.TotalBalance.ShouldBe(500);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(500);
        bob.TotalExpensePaidAmount.ShouldBe(0);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(500);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(0);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(500);
        bob.TotalBalance.ShouldBe(-500);
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
                    .WithPercentualShare(10)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithPercentualShare(90)
                )
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .Build();
        var dto =new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(1000);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(1000);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(100);
        alice.TotalExpensePaidAmount.ShouldBe(1000);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(900);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(0);
        alice.TotalBalance.ShouldBe(900);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(900);
        bob.TotalExpensePaidAmount.ShouldBe(0);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(900);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(0);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(900);
        bob.TotalBalance.ShouldBe(-900);
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
                    .WithPercentualShare(10)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithPercentualShare(90)
                )
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .Build();
        var dto =new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(1000);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(1000);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(100);
        alice.TotalExpensePaidAmount.ShouldBe(1000);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(900);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(0);
        alice.TotalBalance.ShouldBe(900);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(900);
        bob.TotalExpensePaidAmount.ShouldBe(0);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(900);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(0);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(900);
        bob.TotalBalance.ShouldBe(-900);

        var charlie = dto.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(0);
        charlie.TotalExpensePaidAmount.ShouldBe(0);
        charlie.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        charlie.TotalAmountOwed.ShouldBe(0);
        charlie.TotalAmountOwedToOtherMembers.ShouldBe(0);
        charlie.TotalBalance.ShouldBe(0);
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
                    .WithPercentualShare(10)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithPercentualShare(90)
                )
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(123)
                .WithSplitType(ExpenseSplitType.Percentual)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithPercentualShare(60)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithPercentualShare(30)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Charlie.Id)
                    .WithPercentualShare(10)
                )
                .WithPaidByMemberId(Members.Bob.Id)
            )
            .Build();
        var dto =new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(1123);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(1123);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(173.8m);
        alice.TotalExpensePaidAmount.ShouldBe(1000);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(73.8m);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(900);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(73.8m);
        alice.TotalBalance.ShouldBe(826.2m);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(936.9m);
        bob.TotalExpensePaidAmount.ShouldBe(123);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(900);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(86.1m);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(900);
        bob.TotalBalance.ShouldBe(-813.9m);

        var charlie = dto.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(12.3m);
        charlie.TotalExpensePaidAmount.ShouldBe(0);
        charlie.TotalExpenseAmountPaidByOtherMembers.ShouldBe(12.3m);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        charlie.TotalAmountOwed.ShouldBe(0);
        charlie.TotalAmountOwedToOtherMembers.ShouldBe(12.3m);
        charlie.TotalBalance.ShouldBe(-12.3m);
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
                    .WithPercentualShare(100)
                )
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .Build();
        var dto =new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(100);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(100);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(100);
        alice.TotalExpensePaidAmount.ShouldBe(100);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(0);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(0);
        alice.TotalBalance.ShouldBe(0);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(0);
        bob.TotalExpensePaidAmount.ShouldBe(0);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(0);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(0);
        bob.TotalBalance.ShouldBe(0);
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
                    .WithPercentualShare(100)
                )
                .WithPaidByMemberId(Members.Bob.Id)
            )
            .Build();
        var dto =new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(100);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(100);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(100);
        alice.TotalExpensePaidAmount.ShouldBe(0);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(100);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(0);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(100);
        alice.TotalBalance.ShouldBe(-100);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(0);
        bob.TotalExpensePaidAmount.ShouldBe(100);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(100);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(0);
        bob.TotalBalance.ShouldBe(100);
    }

    #endregion

    #region SplitType_ExactAmount

    [Test]
    public void SplitExactly_TotalAmountsPerMember1()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(1000m)
                .WithSplitType(ExpenseSplitType.ExactAmount)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithExactShare(500)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithExactShare(500)
                )
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .Build();
        var dto =new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(1000);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(1000);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(500);
        alice.TotalExpensePaidAmount.ShouldBe(1000);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(500);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(0);
        alice.TotalBalance.ShouldBe(500);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(500);
        bob.TotalExpensePaidAmount.ShouldBe(0);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(500);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(0);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(500);
        bob.TotalBalance.ShouldBe(-500);
    }

    [Test]
    public void SplitExactly_TotalAmountsPerMember2()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(1000m)
                .WithSplitType(ExpenseSplitType.ExactAmount)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithExactShare(100)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithExactShare(900)
                )
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .Build();
        var dto =new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(1000);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(1000);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(100);
        alice.TotalExpensePaidAmount.ShouldBe(1000);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(900);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(0);
        alice.TotalBalance.ShouldBe(900);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(900);
        bob.TotalExpensePaidAmount.ShouldBe(0);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(900);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(0);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(900);
        bob.TotalBalance.ShouldBe(-900);
    }

    [Test]
    public void SplitExactly_TotalAmountsPerMember3()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(1000m)
                .WithSplitType(ExpenseSplitType.ExactAmount)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithExactShare(100)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithExactShare(900)
                )
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .Build();
        var dto =new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(1000);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(1000);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(100);
        alice.TotalExpensePaidAmount.ShouldBe(1000);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(900);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(0);
        alice.TotalBalance.ShouldBe(900);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(900);
        bob.TotalExpensePaidAmount.ShouldBe(0);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(900);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(0);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(900);
        bob.TotalBalance.ShouldBe(-900);

        var charlie = dto.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(0);
        charlie.TotalExpensePaidAmount.ShouldBe(0);
        charlie.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        charlie.TotalAmountOwed.ShouldBe(0);
        charlie.TotalAmountOwedToOtherMembers.ShouldBe(0);
        charlie.TotalBalance.ShouldBe(0);
    }

    [Test]
    public void SplitExactly_TotalAmountsPerMember4()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(1000m)
                .WithSplitType(ExpenseSplitType.ExactAmount)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithExactShare(100)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithExactShare(900)
                )
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(123)
                .WithSplitType(ExpenseSplitType.ExactAmount)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithExactShare(73.8m)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithExactShare(36.9m)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Charlie.Id)
                    .WithExactShare(12.3m)
                )
                .WithPaidByMemberId(Members.Bob.Id)
            )
            .Build();
        var dto =new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(1123);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(1123);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(173.8m);
        alice.TotalExpensePaidAmount.ShouldBe(1000);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(73.8m);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(900);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(73.8m);
        alice.TotalBalance.ShouldBe(826.2m);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(936.9m);
        bob.TotalExpensePaidAmount.ShouldBe(123);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(900);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(86.1m);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(900);
        bob.TotalBalance.ShouldBe(-813.9m);

        var charlie = dto.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(12.3m);
        charlie.TotalExpensePaidAmount.ShouldBe(0);
        charlie.TotalExpenseAmountPaidByOtherMembers.ShouldBe(12.3m);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        charlie.TotalAmountOwed.ShouldBe(0);
        charlie.TotalAmountOwedToOtherMembers.ShouldBe(12.3m);
        charlie.TotalBalance.ShouldBe(-12.3m);
    }

    [Test]
    public void SplitExactly_TotalAmountsPerMember5()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithSplitType(ExpenseSplitType.ExactAmount)
                .WithAmount(100m)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithExactShare(100)
                )
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .Build();
        var dto =new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(100);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(100);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(100);
        alice.TotalExpensePaidAmount.ShouldBe(100);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(0);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(0);
        alice.TotalBalance.ShouldBe(0);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(0);
        bob.TotalExpensePaidAmount.ShouldBe(0);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(0);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(0);
        bob.TotalBalance.ShouldBe(0);
    }

    [Test]
    public void SplitExactly_TotalAmountsPerMember6()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithSplitType(ExpenseSplitType.ExactAmount)
                .WithAmount(100m)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithExactShare(100)
                )
                .WithPaidByMemberId(Members.Bob.Id)
            )
            .Build();
        var dto =new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(100);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(100);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(100);
        alice.TotalExpensePaidAmount.ShouldBe(0);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(100);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(0);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(100);
        alice.TotalBalance.ShouldBe(-100);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(0);
        bob.TotalExpensePaidAmount.ShouldBe(100);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(0);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(100);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(0);
        bob.TotalBalance.ShouldBe(100);
    }

    #endregion

    #region SplitType_Mixed

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
                    .WithPercentualShare(20)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithPercentualShare(80)
                )
                .WithPaidByMemberId(Members.Bob.Id)
            )
            .Build();
        var dto =new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(1900);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(1900);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        Math.Round(alice.TotalExpenseAmount, 2).ShouldBe(513.33m);
        alice.TotalExpensePaidAmount.ShouldBe(1000);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(180);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        Math.Round(alice.TotalAmountOwed, 2).ShouldBe(666.67m);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(180);
        Math.Round(alice.TotalBalance, 2).ShouldBe(486.67m);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        Math.Round(bob.TotalExpenseAmount, 2).ShouldBe(1053.33m);
        bob.TotalExpensePaidAmount.ShouldBe(900);
        Math.Round(bob.TotalExpenseAmountPaidByOtherMembers, 2).ShouldBe(333.33m);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(180);
        Math.Round(bob.TotalAmountOwedToOtherMembers, 2).ShouldBe(333.33m);
        Math.Round(bob.TotalBalance, 2).ShouldBe(-153.33m);

        var charlie = dto.Members.Single(m => m.Id == Members.Charlie.Id);
        Math.Round(charlie.TotalExpenseAmount, 2).ShouldBe(333.33m);
        charlie.TotalExpensePaidAmount.ShouldBe(0);
        Math.Round(charlie.TotalExpenseAmountPaidByOtherMembers, 2).ShouldBe(333.33m);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        charlie.TotalAmountOwed.ShouldBe(0);
        Math.Round(charlie.TotalAmountOwedToOtherMembers, 2).ShouldBe(333.33m);
        Math.Round(charlie.TotalBalance, 2).ShouldBe(-333.33m);
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
                    .WithExactShare(300)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithExactShare(400)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Charlie.Id)
                    .WithExactShare(200)
                )
                .WithPaidByMemberId(Members.Charlie.Id)
            )
            .Build();
        var dto =new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(1500);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(1500);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(500);
        alice.TotalExpensePaidAmount.ShouldBe(600);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(300);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(400);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(300);
        alice.TotalBalance.ShouldBe(100);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(600);
        bob.TotalExpensePaidAmount.ShouldBe(0);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(600);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(0);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(600);
        bob.TotalBalance.ShouldBe(-600);

        var charlie = dto.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(400);
        charlie.TotalExpensePaidAmount.ShouldBe(900);
        charlie.TotalExpenseAmountPaidByOtherMembers.ShouldBe(200);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        charlie.TotalAmountOwed.ShouldBe(700);
        charlie.TotalAmountOwedToOtherMembers.ShouldBe(200);
        charlie.TotalBalance.ShouldBe(500);
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
                    .WithPercentualShare(50)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithPercentualShare(30)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Charlie.Id)
                    .WithPercentualShare(20)
                )
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(500m)
                .WithSplitType(ExpenseSplitType.ExactAmount)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithExactShare(200)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithExactShare(300)
                )
                .WithPaidByMemberId(Members.Bob.Id)
            )
            .Build();
        var dto =new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(800);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(800);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(350);
        alice.TotalExpensePaidAmount.ShouldBe(300);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(200);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(150);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(200);
        alice.TotalBalance.ShouldBe(-50);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(390);
        bob.TotalExpensePaidAmount.ShouldBe(500);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(90);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(200);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(90);
        bob.TotalBalance.ShouldBe(110);

        var charlie = dto.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(60);
        charlie.TotalExpensePaidAmount.ShouldBe(0);
        charlie.TotalExpenseAmountPaidByOtherMembers.ShouldBe(60);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        charlie.TotalAmountOwed.ShouldBe(0);
        charlie.TotalAmountOwedToOtherMembers.ShouldBe(60);
        charlie.TotalBalance.ShouldBe(-60);
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
            .Build();
        var dto =new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(1200);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(1200);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(350);
        alice.TotalExpensePaidAmount.ShouldBe(300);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(250);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(200);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(250);
        alice.TotalBalance.ShouldBe(-50);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(350);
        bob.TotalExpensePaidAmount.ShouldBe(400);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(250);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(300);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(250);
        bob.TotalBalance.ShouldBe(50);

        var charlie = dto.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(500);
        charlie.TotalExpensePaidAmount.ShouldBe(500);
        charlie.TotalExpenseAmountPaidByOtherMembers.ShouldBe(300);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        charlie.TotalAmountOwed.ShouldBe(300);
        charlie.TotalAmountOwedToOtherMembers.ShouldBe(300);
        charlie.TotalBalance.ShouldBe(0);
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
                    .WithPercentualShare(30)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithPercentualShare(20)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Charlie.Id)
                    .WithPercentualShare(50)
                )
                .WithPaidByMemberId(Members.Bob.Id)
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(750m)
                .WithSplitType(ExpenseSplitType.ExactAmount)
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithExactShare(250)
                )
                .AddParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Charlie.Id)
                    .WithExactShare(500)
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
        var dto =new GroupDto(group);

        dto.TotalExpenseAmount.ShouldBe(2800);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.TotalBalance.ShouldBe(2800);

        var alice = dto.Members.Single(m => m.Id == Members.Alice.Id);
        alice.TotalExpenseAmount.ShouldBe(930);
        alice.TotalExpensePaidAmount.ShouldBe(1450);
        alice.TotalExpenseAmountPaidByOtherMembers.ShouldBe(430);
        alice.TotalPaymentReceivedAmount.ShouldBe(0);
        alice.TotalPaymentSentAmount.ShouldBe(0);
        alice.TotalAmountOwed.ShouldBe(950);
        alice.TotalAmountOwedToOtherMembers.ShouldBe(430);
        alice.TotalBalance.ShouldBe(520);

        var bob = dto.Members.Single(m => m.Id == Members.Bob.Id);
        bob.TotalExpenseAmount.ShouldBe(845);
        bob.TotalExpensePaidAmount.ShouldBe(600);
        bob.TotalExpenseAmountPaidByOtherMembers.ShouldBe(725);
        bob.TotalPaymentReceivedAmount.ShouldBe(0);
        bob.TotalPaymentSentAmount.ShouldBe(0);
        bob.TotalAmountOwed.ShouldBe(480);
        bob.TotalAmountOwedToOtherMembers.ShouldBe(725);
        bob.TotalBalance.ShouldBe(-245);

        var charlie = dto.Members.Single(m => m.Id == Members.Charlie.Id);
        charlie.TotalExpenseAmount.ShouldBe(1025);
        charlie.TotalExpensePaidAmount.ShouldBe(750);
        charlie.TotalExpenseAmountPaidByOtherMembers.ShouldBe(525);
        charlie.TotalPaymentReceivedAmount.ShouldBe(0);
        charlie.TotalPaymentSentAmount.ShouldBe(0);
        charlie.TotalAmountOwed.ShouldBe(250);
        charlie.TotalAmountOwedToOtherMembers.ShouldBe(525);
        charlie.TotalBalance.ShouldBe(-275);
    }

    #endregion

    #endregion
}