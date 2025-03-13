using Shouldly;
using SplitTheBillPocV4.Models;
using SplitTheBillPocV4.Modules;
using SplitTheBillPocV4.Tests.TestData;
using SplitTheBillPocV4.Tests.TestData.Builders;

namespace SplitTheBillPocV4.Tests;

internal sealed class MixedScenarioTests
{
    // [Test]
    // public void FullScenario1()
    // {
    //     var group = new GroupBuilder()
    //         .WithMembers([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
    //         .AddExpense(new ExpenseBuilder()
    //             .WithAmount(1000m)
    //             .WithSplitType(ExpenseSplitType.Evenly)
    //             .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity(), Members.Charlie.Entity()])
    //             .WithPaidByMemberId(Members.Alice.Id)
    //         )
    //         .AddExpense(new ExpenseBuilder()
    //             .WithAmount(900m)
    //             .WithSplitType(ExpenseSplitType.Percentual)
    //             .AddParticipant(new ExpenseParticipantBuilder()
    //                 .WithMemberId(Members.Alice.Id)
    //                 .WithPercentualSplitShare(20)
    //             )
    //             .AddParticipant(new ExpenseParticipantBuilder()
    //                 .WithMemberId(Members.Bob.Id)
    //                 .WithPercentualSplitShare(80)
    //             )
    //             .WithPaidByMemberId(Members.Bob.Id)
    //         )
    //         .AddPayment(new PaymentBuilder()
    //             .WithAmount(500)
    //             .WithReceivingMemberId())
    //         .Build();
    //     var model = new EnrichedGroupModel(group);
    //     
    //     model.TotalExpenseAmount.ShouldBe(1900);
    //     model.TotalPaymentAmount.ShouldBe(0);
    //     model.TotalAmountDue.ShouldBe(1900);
    //     
    // }
}