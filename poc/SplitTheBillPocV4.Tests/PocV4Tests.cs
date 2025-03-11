using Shouldly;
using SplitTheBillPocV4.Modules;
using SplitTheBillPocV4.Tests.TestData;
using SplitTheBillPocV4.Tests.TestData.Builders;

namespace SplitTheBillPocV4.Tests;

internal sealed class PocV4Tests
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

    [Test]
    public void PaidByMember_HasNegativeAmountDueBalance()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
            .AddExpense(new ExpenseBuilder()
                .WithAmount(1500m)
                .WithParticipants([
                    new ExpenseParticipantBuilder()
                        .WithMemberId(Members.Alice.Id),
                    new ExpenseParticipantBuilder()
                        .WithMemberId(Members.Bob.Id),
                    new ExpenseParticipantBuilder()
                        .WithMemberId(Members.Charlie.Id),
                ])
                .WithPaidByMemberId(Members.Alice.Id)
            )
            // .AddPayment(new PaymentBuilder()
            //     .WithAmount(1500m)
            //     .WithMemberId(Members.Alice.Id))
            .Build();
        var dto = group.MapToDetailedGroupDTO();
        
        dto.TotalExpenseAmount.ShouldBe(1500m);
        // dto.TotalPaymentAmount.ShouldBe(1500m);
        // dto.AmountDue.ShouldBe(0);
        dto.AmountsDueByMember[Members.Alice.Id].ShouldBe(-1000m);
        dto.AmountsDueByMember[Members.Bob.Id].ShouldBe(500m);
        dto.AmountsDueByMember[Members.Charlie.Id].ShouldBe(500m);
    }
}