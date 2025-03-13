using Shouldly;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.UnitTests.TestData;
using SplitTheBill.Application.UnitTests.TestData.Builders;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.UnitTests.Modules.Groups;

internal sealed class GroupDtoTests
{
    [Test]
    public void GroupDto_MapsGroupProperties()
    {
        var id = new Guid("62F29851-240D-4CAF-8543-7A1DA8EAE192");
        const string name = "group name";
        var group = new GroupBuilder()
            .WithId(id)
            .WithName(name)
            .Build();
        var dto = new GroupDto(group);

        dto.Id.ShouldBe(id);
        dto.Name.ShouldBe(name);
    }

    [Test]
    public void GroupDto_MapsMemberProperties()
    {
        var group = new GroupBuilder()
            .WithMembers([Members.Alice.Entity(), Members.Bob.Entity()])
            .Build();
        var dto = new GroupDto(group);

        dto.Members.Count.ShouldBe(2);
        dto.Members
            .Where(m => m.Id == Members.Alice.Id)
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                m => m.Name.ShouldBe(Members.Alice.Name)
            );
        dto.Members
            .Where(m => m.Id == Members.Bob.Id)
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                m => m.Name.ShouldBe(Members.Bob.Name)
            );
    }

    [Test]
    public void GroupDto_MapsPaymentProperties()
    {
        var payment1Id = new Guid("B860D02D-F104-42B6-8D86-7F0700D4A8A3");

        var group = new GroupBuilder()
            .WithPayment(new PaymentBuilder()
                .WithId(payment1Id)
                .WithSendingMemberId(Members.Alice.Id)
                .WithReceivingMemberId(Members.Bob.Id)
                .WithAmount(1m)
            )
            .Build();
        var dto = new GroupDto(group);

        dto.Payments.ShouldHaveSingleItem();
        dto.Payments
            .Where(p => p.Id == payment1Id)
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                p => p.SendingMemberId.ShouldBe(Members.Alice.Id),
                p => p.ReceivingMemberId.ShouldBe(Members.Bob.Id),
                p => p.Amount.ShouldBe(1m)
            );
    }

    [Test]
    public void GroupDto_MapsExpenseProperties()
    {
        var expense1Id = new Guid("104BF052-7B7F-4F78-8C8A-9BE4B44BD6AD");
        var expense2Id = new Guid("0A17617A-6909-422F-A3B2-CD126DC86CD2");
        var expense3Id = new Guid("C2D5EB5E-1E2F-4FBF-BEB1-068F282EE1F0");

        var group = new GroupBuilder()
            .WithExpense(new ExpenseBuilder()
                .WithId(expense1Id)
                .WithDescription("expense 1")
                .WithAmount(1)
                .WithSplitType(ExpenseSplitType.Evenly)
                .WithParticipants([Members.Alice.Entity(), Members.Bob.Entity()])
                .WithPaidByMemberId(Members.Alice.Id)
            )
            .WithExpense(new ExpenseBuilder()
                .WithId(expense2Id)
                .WithDescription("expense 2")
                .WithAmount(2)
                .WithSplitType(ExpenseSplitType.Percentual)
                .WithParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithPercentualShare(60)
                )
                .WithParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithPercentualShare(40)
                )
                .WithPaidByMemberId(Members.Bob.Id)
            )
            .WithExpense(new ExpenseBuilder()
                .WithId(expense3Id)
                .WithDescription("expense 3")
                .WithAmount(3)
                .WithSplitType(ExpenseSplitType.ExactAmount)
                .WithParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Alice.Id)
                    .WithExactShare(2)
                )
                .WithParticipant(new ExpenseParticipantBuilder()
                    .WithMemberId(Members.Bob.Id)
                    .WithExactShare(1)
                )
                .WithPaidByMemberId(Members.Charlie.Id)
            )
            .Build();
        var dto = new GroupDto(group);

        dto.Expenses.Count.ShouldBe(3);
        dto.Expenses
            .Where(e => e.Id == expense1Id)
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe("expense 1"),
                e => e.Amount.ShouldBe(1),
                e => e.SplitType.ShouldBe(ExpenseSplitType.Evenly),
                e => e.PaidByMemberId.ShouldBe(Members.Alice.Id),
                e => e.Participants.Count.ShouldBe(2),
                e => e.Participants
                    .Where(p => p.MemberId == Members.Alice.Id)
                    .ShouldHaveSingleItem(),
                e => e.Participants
                    .Where(p => p.MemberId == Members.Bob.Id)
                    .ShouldHaveSingleItem()
            );
        dto.Expenses
            .Where(e => e.Id == expense2Id)
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe("expense 2"),
                e => e.Amount.ShouldBe(2),
                e => e.SplitType.ShouldBe(ExpenseSplitType.Percentual),
                e => e.PaidByMemberId.ShouldBe(Members.Bob.Id),
                e => e.Participants.Count.ShouldBe(2),
                e => e.Participants
                    .Where(p => p.MemberId == Members.Alice.Id)
                    .ShouldHaveSingleItem()
                    .ShouldSatisfyAllConditions(
                        p => p.PercentualShare.ShouldBe(60)
                    ),
                e => e.Participants
                    .Where(p => p.MemberId == Members.Bob.Id)
                    .ShouldHaveSingleItem()
                    .ShouldSatisfyAllConditions(
                        p => p.PercentualShare.ShouldBe(40)
                    )
            );
        dto.Expenses
            .Where(e => e.Id == expense3Id)
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe("expense 3"),
                e => e.Amount.ShouldBe(3),
                e => e.SplitType.ShouldBe(ExpenseSplitType.ExactAmount),
                e => e.PaidByMemberId.ShouldBe(Members.Charlie.Id),
                e => e.Participants.Count.ShouldBe(2),
                e => e.Participants
                    .Where(p => p.MemberId == Members.Alice.Id)
                    .ShouldHaveSingleItem()
                    .ShouldSatisfyAllConditions(
                        p => p.ExactShare.ShouldBe(2)
                    ),
                e => e.Participants
                    .Where(p => p.MemberId == Members.Bob.Id)
                    .ShouldHaveSingleItem()
                    .ShouldSatisfyAllConditions(
                        p => p.ExactShare.ShouldBe(1)
                    )
            );
    }
}