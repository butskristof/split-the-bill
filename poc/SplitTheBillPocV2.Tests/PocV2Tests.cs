using Shouldly;
using SplitTheBillPocV2.Modules;
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
        dto.ExpenseAmountPerMember.ShouldBe(0);
        dto.AmountsDueByMember.Values.ShouldAllBe(v => v == 0);
    }

    [Test]
    public void ExpenseForAll_SpreadEvenly()
    {
        true.ShouldBeFalse();
    }

    [Test]
    public void ExpenseForOne_CountedPersonally()
    {
        true.ShouldBeFalse();
    }

    [Test]
    public void ExpenseForSome_AttributedToSome()
    {
        true.ShouldBeFalse();
    }

    [Test]
    public void ExpenseForAll_AddMember_HasNoAmountDue()
    {
        true.ShouldBeFalse();
    }
}