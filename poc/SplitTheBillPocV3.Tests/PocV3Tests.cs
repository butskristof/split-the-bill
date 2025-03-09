using Shouldly;
using SplitTheBillPocV3.Models;
using SplitTheBillPocV3.Modules;
using SplitTheBillPocV3.Tests.TestData;
using SplitTheBillPocV3.Tests.TestData.Builders;

namespace SplitTheBillPocV3.Tests;

public class PocV3Tests
{
    [Test]
    public void EmptyGroup_ZeroAmounts()
    {
        var group = new GroupBuilder().Build();
        var dto = group.MapToDetailedGroupDTO();
        dto.TotalExpenseAmount.ShouldBe(0);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.AmountDue.ShouldBe(0);
        dto.AmountsDueByMember.Values.ShouldAllBe(v => v == 0);
    }

    [Test]
    public void ExpenseForAll_SpreadEvenly()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(100m)
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity()])
                .Build())
            .AddExpense(new ExpenseBuilder()
                .WithAmount(200m)
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity()])
                .Build())
            .Build();
        var dto = group.MapToDetailedGroupDTO();

        dto.TotalExpenseAmount.ShouldBe(300m);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.AmountDue.ShouldBe(300m);
        dto.AmountsDueByMember.Values.ShouldAllBe(v => v == 150m);
    }

    [Test]
    public void ExpenseForOne_CountedPersonally()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(100m)
                .WithParticipants([Members.Alice.Entity()])
                .Build())
            .Build();
        var dto = group.MapToDetailedGroupDTO();

        dto.TotalExpenseAmount.ShouldBe(100m);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.AmountDue.ShouldBe(100m);
        dto.AmountsDueByMember[Members.Alice.Id].ShouldBe(100m);
        dto.AmountsDueByMember[Members.Bob.Id].ShouldBe(0);
    }

    [Test]
    public void ExpenseForSome_AttributedToSome()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(100m)
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity()])
                .Build())
            .Build();
        var dto = group.MapToDetailedGroupDTO();

        dto.TotalExpenseAmount.ShouldBe(100m);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.AmountDue.ShouldBe(100m);
        dto.AmountsDueByMember[Members.Alice.Id].ShouldBe(50m);
        dto.AmountsDueByMember[Members.Bob.Id].ShouldBe(50m);
        dto.AmountsDueByMember[Members.Charlie.Id].ShouldBe(0);
    }

    [Test]
    public void ExpenseForAll_AddMember_HasNoAmountDue()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(100m)
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity()])
                .Build())
            .Build();
        group.Members.Add(Members.Charlie.Entity());
        var dto = group.MapToDetailedGroupDTO();

        dto.TotalExpenseAmount.ShouldBe(100m);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.AmountDue.ShouldBe(100m);
        dto.AmountsDueByMember[Members.Alice.Id].ShouldBe(50m);
        dto.AmountsDueByMember[Members.Bob.Id].ShouldBe(50m);
        dto.AmountsDueByMember[Members.Charlie.Id].ShouldBe(0);
    }

    [Test]
    public void ExpenseSplitPercentually()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(100m)
                .WithSplitType(ExpenseSplitType.Percentual)
                .WithParticipants([
                    new ExpenseParticipantBuilder()
                        .WithMemberId(Members.Alice.Id)
                        .WithPercentualSplitShare(0.6),
                    new ExpenseParticipantBuilder()
                        .WithMemberId(Members.Bob.Id)
                        .WithPercentualSplitShare(0.4),
                ])
                .Build())
            .Build();
        var dto = group.MapToDetailedGroupDTO();

        dto.TotalExpenseAmount.ShouldBe(100m);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.AmountDue.ShouldBe(100m);
        dto.AmountsDueByMember[Members.Alice.Id].ShouldBe(60m);
        dto.AmountsDueByMember[Members.Bob.Id].ShouldBe(40m);
    }

    [Test]
    public void ExpenseSplitExactly()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(100m)
                .WithSplitType(ExpenseSplitType.ExactAmount)
                .WithParticipants([
                    new ExpenseParticipantBuilder()
                        .WithMemberId(Members.Alice.Id)
                        .WithExactAmountSplitShare(70),
                    new ExpenseParticipantBuilder()
                        .WithMemberId(Members.Bob.Id)
                        .WithExactAmountSplitShare(30)
                ])
                .Build())
            .Build();
        var dto = group.MapToDetailedGroupDTO();

        dto.TotalExpenseAmount.ShouldBe(100m);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.AmountDue.ShouldBe(100m);
        dto.AmountsDueByMember[Members.Alice.Id].ShouldBe(70m);
        dto.AmountsDueByMember[Members.Bob.Id].ShouldBe(30m);
    }

    [Test]
    public void MultipleExpenseSplitTypes()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(500m)
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity()])
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(500m)
                .WithSplitType(ExpenseSplitType.Percentual)
                .WithParticipants([
                    new ExpenseParticipantBuilder()
                        .WithMemberId(Members.Alice.Id)
                        .WithPercentualSplitShare(0.1),
                    new ExpenseParticipantBuilder()
                        .WithMemberId(Members.Bob.Id)
                        .WithPercentualSplitShare(0.9),
                ])
            )
            .AddExpense(new ExpenseBuilder()
                .WithAmount(500m)
                .WithSplitType(ExpenseSplitType.ExactAmount)
                .WithParticipants([
                    new ExpenseParticipantBuilder()
                        .WithMemberId(Members.Alice.Id)
                        .WithExactAmountSplitShare(400),
                    new ExpenseParticipantBuilder()
                        .WithMemberId(Members.Bob.Id)
                        .WithExactAmountSplitShare(100),
                ])
            )
            .Build();
        var dto = group.MapToDetailedGroupDTO();

        dto.TotalExpenseAmount.ShouldBe(1500m);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.AmountDue.ShouldBe(1500m);
        dto.AmountsDueByMember[Members.Alice.Id].ShouldBe(700m);
        dto.AmountsDueByMember[Members.Bob.Id].ShouldBe(800m);
    }
}