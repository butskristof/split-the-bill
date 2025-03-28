using ErrorOr;
using Shouldly;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.Tests.Shared.Builders;
using SplitTheBill.Application.Tests.Shared.TestData;
using SplitTheBill.Application.Tests.Shared.TestData.Builders;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.IntegrationTests.Modules.Groups.Payments;

internal sealed class CreatePaymentTests() : ApplicationTestBase(true)
{
    [Test]
    public async Task InvalidRequest_ReturnsValidationErrors()
    {
        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(Guid.Empty)
            .WithSendingMemberId(null)
            .WithReceivingMemberId(Guid.Empty)
            .WithAmount(-1)
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList.Count.ShouldBe(4);
        result.ErrorsOrEmptyList
            .ShouldContain(r =>
                r.Type == ErrorType.Validation &&
                r.Code == nameof(request.GroupId) &&
                r.Description == ErrorCodes.Invalid);
        result.ErrorsOrEmptyList
            .ShouldContain(r =>
                r.Type == ErrorType.Validation &&
                r.Code == nameof(request.SendingMemberId) &&
                r.Description == ErrorCodes.Required);
        result.ErrorsOrEmptyList
            .ShouldContain(r =>
                r.Type == ErrorType.Validation &&
                r.Code == nameof(request.ReceivingMemberId) &&
                r.Description == ErrorCodes.Invalid);
        result.ErrorsOrEmptyList
            .ShouldContain(r =>
                r.Type == ErrorType.Validation &&
                r.Code == nameof(request.Amount) &&
                r.Description == ErrorCodes.Invalid);
    }

    [Test]
    public async Task GroupDoesNotExist_ReturnsNotFoundError()
    {
        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(Guid.NewGuid())
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.GroupId))
            );
    }

    [Test]
    public async Task SendingMemberDoesNotExist_ReturnsNotFoundError()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .Build()
        );
        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(groupId)
            .WithSendingMemberId(Guid.NewGuid())
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.SendingMemberId))
            );
    }

    [Test]
    public async Task SendingMemberNotInGroup_ReturnsNotFoundError()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .Build()
        );

        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(groupId)
            .WithSendingMemberId(TestMembers.Alice.Id)
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.SendingMemberId))
            );
    }

    [Test]
    public async Task ReceivingMemberDoesNotExist_ReturnsNotFoundError()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id])
                .Build()
        );

        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(groupId)
            .WithSendingMemberId(TestMembers.Alice.Id)
            .WithReceivingMemberId(Guid.NewGuid())
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.ReceivingMemberId))
            );
    }

    [Test]
    public async Task ReceivingMemberNotInGroup_ReturnsNotFoundError()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id])
                .Build()
        );

        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(groupId)
            .WithSendingMemberId(TestMembers.Alice.Id)
            .WithReceivingMemberId(TestMembers.Bob.Id)
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.ReceivingMemberId))
            );
    }

    [Test]
    public async Task ValidPaymentRequest_ReturnsCreated()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([
                    TestMembers.Alice.Id,
                    TestMembers.Bob.Id
                ])
                .Build()
        );

        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(groupId)
            .WithSendingMemberId(TestMembers.Alice.Id)
            .WithReceivingMemberId(TestMembers.Bob.Id)
            .WithAmount(100m)
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Created>();
    }

    [Test]
    public async Task ValidPaymentRequest_PersistsPayment()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([
                    TestMembers.Alice.Id,
                    TestMembers.Bob.Id
                ])
                .Build()
        );

        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(groupId)
            .WithSendingMemberId(TestMembers.Alice.Id)
            .WithReceivingMemberId(TestMembers.Bob.Id)
            .WithAmount(100m)
            .Build();
        await Application.SendAsync(request);

        var group = await Application
            .FindAsync<Group>(
                g => g.Id == groupId,
                g => g.Payments
            );
        group!.Payments
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                p => p.SendingMemberId.ShouldBe(TestMembers.Alice.Id),
                p => p.ReceivingMemberId.ShouldBe(TestMembers.Bob.Id),
                p => p.Amount.ShouldBe(100)
            );
    }

    [Test]
    public async Task ValidPaymentRequest_IsReturnedInGroupDtoAndUpdatesTotals()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([
                    TestMembers.Alice.Id,
                    TestMembers.Bob.Id
                ])
                .Build()
        );

        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(groupId)
            .WithSendingMemberId(TestMembers.Alice.Id)
            .WithReceivingMemberId(TestMembers.Bob.Id)
            .WithAmount(100m)
            .Build();
        await Application.SendAsync(request);

        var result = await Application.SendAsync(new GetGroup.Request(groupId));
        var response = result.Value;
        response.TotalPaymentAmount.ShouldBe(100m);
        response.Payments
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                p => p.SendingMemberId.ShouldBe(TestMembers.Alice.Id),
                p => p.ReceivingMemberId.ShouldBe(TestMembers.Bob.Id),
                p => p.Amount.ShouldBe(100)
            );
    }

    // TODO creating member not in group
}