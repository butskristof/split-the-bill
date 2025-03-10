using Shouldly;
using SplitTheBillPocV4.Modules;
using SplitTheBillPocV4.Tests.TestData.Builders;

namespace SplitTheBillPocV4.Tests;

public class PocV4Tests
{
    [Test]
    public void EmptyGroup_ZeroAmounts()
    {
        var group = new GroupBuilder()
            .Build();
        var dto = group.MapToDetailedGroupDTO();
        
        dto.TotalExpenseAmount.ShouldBe(0);
        dto.TotalPaymentAmount.ShouldBe(0);
        dto.AmountDue.ShouldBe(0);
        dto.AmountsDueByMember.Values.ShouldAllBe(v => v == 0);
    }
}