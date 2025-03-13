using Shouldly;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.UnitTests.TestData;
using SplitTheBill.Application.UnitTests.TestData.Builders;

namespace SplitTheBill.Application.UnitTests.Modules.Groups;

internal sealed class GroupDtoTests
{
    [Test]
    public void GroupDto_MapsProperties()
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
    public void GroupDto_MapsPayments()
    {
        var payment1Id = new Guid("B860D02D-F104-42B6-8D86-7F0700D4A8A3");
        var payment2Id = new Guid("B279BACB-DFF6-486E-84B8-C8D02C97B685");
        var payment3Id = new Guid("06ABB825-1518-4DC6-A225-ED5AE608F74E");
        
        var group = new GroupBuilder()
            .WithPayment(new PaymentBuilder()
                .WithId(payment1Id)
                .WithSendingMemberId(Members.Alice.Id)
                .WithReceivingMemberId(Members.Bob.Id)
                .WithAmount(1m)
            )
            .WithPayment(new PaymentBuilder()
                .WithId(payment2Id)
                .WithSendingMemberId(Members.Bob.Id)
                .WithReceivingMemberId(Members.Alice.Id)
                .WithAmount(100m)
            )
            .WithPayment(new PaymentBuilder()
                .WithId(payment3Id)
                .WithSendingMemberId(Members.Alice.Id)
                .WithReceivingMemberId(Members.Bob.Id)
                .WithAmount(123.45m)
            )
            .Build();
        var dto = new GroupDto(group);

        dto.Payments.Count.ShouldBe(3);
        dto.Payments
            .Where(p => p.Id == payment1Id)
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                p => p.SendingMemberId.ShouldBe(Members.Alice.Id),
                p => p.ReceivingMemberId.ShouldBe(Members.Bob.Id),
                p => p.Amount.ShouldBe(1m)
            );
        dto.Payments
            .Where(p => p.Id == payment2Id)
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                p => p.SendingMemberId.ShouldBe(Members.Bob.Id),
                p => p.ReceivingMemberId.ShouldBe(Members.Alice.Id),
                p => p.Amount.ShouldBe(100m)
            );
        dto.Payments
            .Where(p => p.Id == payment3Id)
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                p => p.SendingMemberId.ShouldBe(Members.Alice.Id),
                p => p.ReceivingMemberId.ShouldBe(Members.Bob.Id),
                p => p.Amount.ShouldBe(123.45m)
            );
    }
}