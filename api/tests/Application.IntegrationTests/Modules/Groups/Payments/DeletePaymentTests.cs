using ErrorOr;
using Shouldly;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.Modules.Groups.Payments;
using SplitTheBill.Application.Tests.Shared.TestData.Builders;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.IntegrationTests.Modules.Groups.Payments;

internal sealed class DeletePaymentTests : ApplicationTestBase
{
    [Test]
    public async Task InvalidRequest_ReturnsValidationError()
    {
        var request = new DeletePayment.Request(
            Guid.Empty,
            new Guid("0295D1E6-2B44-4B1C-BE5C-824D97F13A99")
        );
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.Validation),
                e => e.Code.ShouldBe(nameof(request.GroupId)),
                e => e.Description.ShouldBe(ErrorCodes.Invalid)
            );
    }

    [Test]
    public async Task GroupDoesNotExist_ReturnsNotFoundError()
    {
        await Application.AddAsync(
            Tests.Shared.TestData.Members.Alice,
            Tests.Shared.TestData.Members.Bob
        );
        await Application.AddAsync(new GroupBuilder()
            .WithId(new Guid("C612FC70-90C6-4D9B-ACEB-8BF81429F6B4"))
            .WithPayments([
                new PaymentBuilder()
                    .WithId(new Guid("A890E7FB-12BD-4833-A443-20104F713E3B"))
                    .WithSendingMemberId(Tests.Shared.TestData.Members.Alice.Id)
                    .WithReceivingMemberId(Tests.Shared.TestData.Members.Bob.Id)
            ])
            .WithName("group name")
            .Build());

        var request = new DeletePayment.Request(
            new Guid("7E32246F-5839-40CF-AEB4-D689F058BC9D"),
            new Guid("F38A683C-E927-45A9-95AB-ABA8E0284D20")
        );
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.GroupId))
            );

        // verify payments are untouched
        var paymentCount = await Application.CountAsync<Payment>();
        paymentCount.ShouldBe(1);
    }

    [Test]
    public async Task PaymentDoesNotExist_ReturnsNotFoundError()
    {
        var groupId = new Guid("C612FC70-90C6-4D9B-ACEB-8BF81429F6B4");
        await Application.AddAsync(
            Tests.Shared.TestData.Members.Alice,
            Tests.Shared.TestData.Members.Bob
        );
        await Application.AddAsync(new GroupBuilder()
            .WithId(groupId)
            .WithPayments([
                new PaymentBuilder()
                    .WithId(new Guid("A890E7FB-12BD-4833-A443-20104F713E3B"))
                    .WithSendingMemberId(Tests.Shared.TestData.Members.Alice.Id)
                    .WithReceivingMemberId(Tests.Shared.TestData.Members.Bob.Id)
            ])
            .WithName("group name")
            .Build());

        var request = new DeletePayment.Request(
            groupId,
            new Guid("D836BFD7-02D2-43A0-9132-37EF96CB9800")
        );
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.PaymentId))
            );

        // verify payments are untouched
        var paymentCount = await Application.CountAsync<Payment>();
        paymentCount.ShouldBe(1);
    }

    [Test]
    public async Task PaymentsExistsInOtherGroup_ReturnsNotFoundError()
    {
        var paymentId = new Guid("141D6CE0-A335-42BB-87DA-9681C5A7EC53");
        var groupId = new Guid("FAE2C9EF-9872-468B-90DD-A902EA034880");
        await Application.AddAsync(
            Tests.Shared.TestData.Members.Alice,
            Tests.Shared.TestData.Members.Bob
        );
        await Application.AddAsync(new GroupBuilder()
            .WithId(new Guid("B9FC4E75-E737-4497-AF5E-4FA6EF05DDBC"))
            .WithPayments([
                new PaymentBuilder()
                    .WithId(paymentId)
                    .WithSendingMemberId(Tests.Shared.TestData.Members.Alice.Id)
                    .WithReceivingMemberId(Tests.Shared.TestData.Members.Bob.Id)
            ])
            .WithName("group name")
            .Build(),
            new GroupBuilder()
                .WithId(groupId)
                .Build());

        var request = new DeletePayment.Request(
            groupId,
            paymentId
        );
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.PaymentId))
            );

        // verify payments are untouched
        var paymentCount = await Application.CountAsync<Payment>();
        paymentCount.ShouldBe(1);
    }

    [Test]
    public async Task DeletesPayment()
    {
        var groupId = new Guid("46EEAE40-21F5-4871-92EA-C52B655E7F3D");
        var paymentId = new Guid("6F0B484F-1880-4287-83C9-32EE0D7F925F");

        await Application.AddAsync(
            Tests.Shared.TestData.Members.Alice,
            Tests.Shared.TestData.Members.Bob
        );
        await Application.AddAsync(new GroupBuilder()
            .WithId(groupId)
            .WithPayments([
                new PaymentBuilder()
                    .WithId(paymentId)
                    .WithSendingMemberId(Tests.Shared.TestData.Members.Alice.Id)
                    .WithReceivingMemberId(Tests.Shared.TestData.Members.Bob.Id)
            ])
            .WithName("group name")
            .Build());

        var request = new DeletePayment.Request(
            groupId,
            paymentId
        );
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Deleted>();

        // verify payment is deleted
        var paymentCount = await Application.CountAsync<Payment>();
        paymentCount.ShouldBe(0);
    }

    [Test]
    public async Task DeletesCorrectPayment()
    {
        var groupId = new Guid("246800F2-B841-4A2C-A220-2BACE1D42B29");
        var payment1Id = new Guid("C06CFBC2-0136-4303-BD05-112D8461226D");
        var payment2Id = new Guid("829001F8-786B-470D-BF3F-FDC9FCFE23E3");

        await Application.AddAsync(
            Tests.Shared.TestData.Members.Alice,
            Tests.Shared.TestData.Members.Bob
        );
        await Application.AddAsync(new GroupBuilder()
            .WithId(groupId)
            .WithPayments([
                new PaymentBuilder()
                    .WithId(payment1Id)
                    .WithSendingMemberId(Tests.Shared.TestData.Members.Alice.Id)
                    .WithReceivingMemberId(Tests.Shared.TestData.Members.Bob.Id),
                new PaymentBuilder()
                    .WithId(payment2Id)
                    .WithSendingMemberId(Tests.Shared.TestData.Members.Alice.Id)
                    .WithReceivingMemberId(Tests.Shared.TestData.Members.Bob.Id)
            ])
            .WithName("group name")
            .Build());

        var request = new DeletePayment.Request(
            groupId,
            payment1Id
        );
        await Application.SendAsync(request);

        // verify payment is deleted
        var paymentCount = await Application.CountAsync<Payment>();
        paymentCount.ShouldBe(1);
        var payment = await Application.FindAsync<Payment>(payment2Id);
        payment.ShouldNotBeNull();
    }

    [Test]
    public async Task IsRemovedFromGroupDtoAndUpdatesTotals()
    {
        var groupId = new Guid("77FB26C4-4764-41D5-AB7B-B2D083B5CD40");
        var payment1Id = new Guid("ED77156D-BD12-4BAF-A68C-1F44C0502DF4");
        var payment2Id = new Guid("845BEA12-F5DB-40D8-B87C-C69BF0EF11E9");

        await Application.AddAsync(
            Tests.Shared.TestData.Members.Alice,
            Tests.Shared.TestData.Members.Bob
        );
        await Application.AddAsync(new GroupBuilder()
            .WithId(groupId)
            .WithPayments([
                new PaymentBuilder()
                    .WithId(payment1Id)
                    .WithAmount(100)
                    .WithSendingMemberId(Tests.Shared.TestData.Members.Alice.Id)
                    .WithReceivingMemberId(Tests.Shared.TestData.Members.Bob.Id),
                new PaymentBuilder()
                    .WithId(payment2Id)
                    .WithAmount(100)
                    .WithSendingMemberId(Tests.Shared.TestData.Members.Alice.Id)
                    .WithReceivingMemberId(Tests.Shared.TestData.Members.Bob.Id)
            ])
            .WithName("group name")
            .Build());

        var request = new DeletePayment.Request(
            groupId,
            payment1Id
        );
        await Application.SendAsync(request);

        var result = await Application.SendAsync(new GetGroup.Request(groupId));
        var response = result.Value;
        response.TotalPaymentAmount.ShouldBe(100m);
        response.Payments
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                p => p.Id.ShouldBe(payment2Id)
            );
    }
}