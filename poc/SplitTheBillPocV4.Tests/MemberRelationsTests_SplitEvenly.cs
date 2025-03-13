using Shouldly;
using SplitTheBillPocV4.Models;
using SplitTheBillPocV4.Modules;
using SplitTheBillPocV4.Tests.TestData;
using SplitTheBillPocV4.Tests.TestData.Builders;

namespace SplitTheBillPocV4.Tests;

internal sealed class MemberRelationsTests_SplitEvenly
{
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
        var model = new EnrichedGroupModel(group);

        var alice = model.Members.Single(m => m.Id == Members.Alice.Id);
        var aliceToBob = alice.Relations[Members.Bob.Id];
        aliceToBob.ExpenseAmountPaidByOtherMemberForThisMember.ShouldBe(0);
        aliceToBob.ExpenseAmountPaidByThisMemberForOtherMember.ShouldBe(500);
        aliceToBob.PaymentAmountReceivedFromOtherMemberToThisMember.ShouldBe(0);
        aliceToBob.PaymentAmountSentToOtherMemberFromThisMember.ShouldBe(0);
        aliceToBob.AmountOwedByThisMemberToOtherMember.ShouldBe(0);
        aliceToBob.AmountOwedToThisMemberByOtherMember.ShouldBe(500);
        aliceToBob.Balance.ShouldBe(500);
        var aliceToCharlie = alice.Relations[Members.Charlie.Id];
        aliceToCharlie.ExpenseAmountPaidByOtherMemberForThisMember.ShouldBe(0);
        aliceToCharlie.ExpenseAmountPaidByThisMemberForOtherMember.ShouldBe(500);
        aliceToCharlie.PaymentAmountReceivedFromOtherMemberToThisMember.ShouldBe(0);
        aliceToCharlie.PaymentAmountSentToOtherMemberFromThisMember.ShouldBe(0);
        aliceToCharlie.AmountOwedByThisMemberToOtherMember.ShouldBe(0);
        aliceToCharlie.AmountOwedToThisMemberByOtherMember.ShouldBe(500);
        aliceToCharlie.Balance.ShouldBe(500);
        
        var bob = model.Members.Single(m => m.Id == Members.Bob.Id);
        var bobToAlice = bob.Relations[Members.Alice.Id];
        bobToAlice.Balance.ShouldBe(-500);
        
        var charlie = model.Members.Single(m => m.Id == Members.Charlie.Id);
        var charlieToAlice = charlie.Relations[Members.Alice.Id];
        charlieToAlice.Balance.ShouldBe(-500);
    }
}