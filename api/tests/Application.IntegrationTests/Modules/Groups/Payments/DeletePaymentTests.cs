using ErrorOr;
using Shouldly;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.Modules.Groups.Payments;
using SplitTheBill.Application.Tests.Shared.TestData;
using SplitTheBill.Application.Tests.Shared.TestData.Builders;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.IntegrationTests.Modules.Groups.Payments;

internal sealed class DeletePaymentTests : ApplicationTestBase
{
    [Test]
    public async Task InvalidRequest_ReturnsValidationError()
    {
        var request = new DeletePayment.Request(Guid.Empty, Guid.NewGuid());
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result
            .ErrorsOrEmptyList.ShouldHaveSingleItem()
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
            new GroupBuilder()
                .WithId(Guid.NewGuid())
                .WithPayments(
                    [
                        new PaymentBuilder()
                            .WithId(Guid.NewGuid())
                            .WithSendingMemberId(TestMembers.Alice.Id)
                            .WithReceivingMemberId(TestMembers.Bob.Id),
                    ]
                )
                .Build()
        );

        var request = new DeletePayment.Request(Guid.NewGuid(), Guid.NewGuid());
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result
            .ErrorsOrEmptyList.ShouldHaveSingleItem()
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
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id, TestMembers.Bob.Id])
                .WithPayments(
                    [
                        new PaymentBuilder()
                            .WithId(Guid.NewGuid())
                            .WithSendingMemberId(TestMembers.Alice.Id)
                            .WithReceivingMemberId(TestMembers.Bob.Id),
                    ]
                )
                .Build()
        );
        Application.SetUserId(TestMembers.Alice.UserId);

        var request = new DeletePayment.Request(groupId, Guid.NewGuid());
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result
            .ErrorsOrEmptyList.ShouldHaveSingleItem()
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
        var paymentId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(Guid.NewGuid())
                .WithDefaultMember()
                .WithPayments(
                    [
                        new PaymentBuilder()
                            .WithId(paymentId)
                            .WithSendingMemberId(TestMembers.Alice.Id)
                            .WithReceivingMemberId(TestMembers.Bob.Id),
                    ]
                )
                .Build(),
            new GroupBuilder().WithId(groupId).WithDefaultMember().Build()
        );

        var request = new DeletePayment.Request(groupId, paymentId);
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result
            .ErrorsOrEmptyList.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.PaymentId))
            );

        // verify payments are untouched
        var paymentCount = await Application.CountAsync<Payment>();
        paymentCount.ShouldBe(1);
    }

    [Test]
    public async Task UserIdNotAGroupMember_ReturnsNotFoundError()
    {
        var groupId = Guid.NewGuid();
        var paymentId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id, TestMembers.Bob.Id])
                .WithPayments(
                    [
                        new PaymentBuilder()
                            .WithId(paymentId)
                            .WithSendingMemberId(TestMembers.Alice.Id)
                            .WithReceivingMemberId(TestMembers.Bob.Id),
                    ]
                )
                .Build()
        );
        Application.SetUserId(TestMembers.Default.UserId);

        var request = new DeletePayment.Request(groupId, paymentId);
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result
            .ErrorsOrEmptyList.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.GroupId))
            );
    }

    [Test]
    public async Task DeletesPayment()
    {
        var groupId = Guid.NewGuid();
        var paymentId = Guid.NewGuid();

        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id, TestMembers.Bob.Id])
                .WithPayments(
                    [
                        new PaymentBuilder()
                            .WithId(paymentId)
                            .WithSendingMemberId(TestMembers.Alice.Id)
                            .WithReceivingMemberId(TestMembers.Bob.Id),
                    ]
                )
                .Build()
        );
        Application.SetUserId(TestMembers.Alice.UserId);

        var request = new DeletePayment.Request(groupId, paymentId);
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
        var groupId = Guid.NewGuid();
        var payment1Id = Guid.NewGuid();
        var payment2Id = Guid.NewGuid();

        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id, TestMembers.Bob.Id])
                .WithPayments(
                    [
                        new PaymentBuilder()
                            .WithId(payment1Id)
                            .WithSendingMemberId(TestMembers.Alice.Id)
                            .WithReceivingMemberId(TestMembers.Bob.Id),
                        new PaymentBuilder()
                            .WithId(payment2Id)
                            .WithSendingMemberId(TestMembers.Alice.Id)
                            .WithReceivingMemberId(TestMembers.Bob.Id),
                    ]
                )
                .Build()
        );
        Application.SetUserId(TestMembers.Alice.UserId);

        var request = new DeletePayment.Request(groupId, payment1Id);
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
        var groupId = Guid.NewGuid();
        var payment1Id = Guid.NewGuid();
        var payment2Id = Guid.NewGuid();

        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id, TestMembers.Bob.Id])
                .WithPayments(
                    [
                        new PaymentBuilder()
                            .WithId(payment1Id)
                            .WithAmount(100)
                            .WithSendingMemberId(TestMembers.Alice.Id)
                            .WithReceivingMemberId(TestMembers.Bob.Id),
                        new PaymentBuilder()
                            .WithId(payment2Id)
                            .WithAmount(100)
                            .WithSendingMemberId(TestMembers.Alice.Id)
                            .WithReceivingMemberId(TestMembers.Bob.Id),
                    ]
                )
                .WithName("group name")
                .Build()
        );
        Application.SetUserId(TestMembers.Alice.UserId);

        var request = new DeletePayment.Request(groupId, payment1Id);
        await Application.SendAsync(request);

        var result = await Application.SendAsync(new GetGroup.Request(groupId));
        var response = result.Value;
        response.TotalPaymentAmount.ShouldBe(100m);
        response
            .Payments.ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(p => p.Id.ShouldBe(payment2Id));
    }
}
