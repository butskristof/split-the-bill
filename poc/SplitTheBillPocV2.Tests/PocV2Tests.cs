using Shouldly;
using SplitTheBillPocV2.Modules;
using SplitTheBillPocV2.Tests.TestData;
using SplitTheBillPocV2.Tests.TestData.Builders;

namespace SplitTheBillPocV2.Tests;

public class PocV2Tests
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
}