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

internal sealed class CreatePaymentTests : ApplicationTestBase
{
    [Test]
    public async Task InvalidRequest_ReturnsValidationErrors()
    {
        var request = new PaymentRequestBuilder()
            .WithGroupId(Guid.Empty)
            .WithSendingMemberId(null)
            .WithReceivingMemberId(Guid.Empty)
            .WithAmount(-1)
            .BuildCreateRequest();
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
        var request = new PaymentRequestBuilder()
            .WithGroupId(Guid.NewGuid())
            .BuildCreateRequest();
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
    public async Task UserIdNotAGroupMember_ReturnsNotFoundError()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id])
                .Build()
        );
        Application.SetUserId(TestMembers.Bob.UserId);
        
        var request = new PaymentRequestBuilder()
            .WithGroupId(groupId)
            .BuildCreateRequest();
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
                .WithDefaultMember()
                .Build()
        );
        var request = new PaymentRequestBuilder()
            .WithGroupId(groupId)
            .WithSendingMemberId(Guid.NewGuid())
            .BuildCreateRequest();
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
                .WithDefaultMember()
                .Build()
        );

        var request = new PaymentRequestBuilder()
            .WithGroupId(groupId)
            .WithSendingMemberId(TestMembers.Alice.Id)
            .BuildCreateRequest();
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
        Application.SetUserId(TestMembers.Alice.UserId);

        var request = new PaymentRequestBuilder()
            .WithGroupId(groupId)
            .WithSendingMemberId(TestMembers.Alice.Id)
            .WithReceivingMemberId(Guid.NewGuid())
            .BuildCreateRequest();
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
        Application.SetUserId(TestMembers.Alice.UserId);

        var request = new PaymentRequestBuilder()
            .WithGroupId(groupId)
            .WithSendingMemberId(TestMembers.Alice.Id)
            .WithReceivingMemberId(TestMembers.Bob.Id)
            .BuildCreateRequest();
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
        Application.SetUserId(TestMembers.Alice.UserId);

        var request = new PaymentRequestBuilder()
            .WithGroupId(groupId)
            .WithSendingMemberId(TestMembers.Alice.Id)
            .WithReceivingMemberId(TestMembers.Bob.Id)
            .WithAmount(100m)
            .BuildCreateRequest();
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
        Application.SetUserId(TestMembers.Alice.UserId);

        var request = new PaymentRequestBuilder()
            .WithGroupId(groupId)
            .WithSendingMemberId(TestMembers.Alice.Id)
            .WithReceivingMemberId(TestMembers.Bob.Id)
            .WithAmount(100m)
            .BuildCreateRequest();
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
        Application.SetUserId(TestMembers.Alice.UserId);

        var request = new PaymentRequestBuilder()
            .WithGroupId(groupId)
            .WithSendingMemberId(TestMembers.Alice.Id)
            .WithReceivingMemberId(TestMembers.Bob.Id)
            .WithAmount(100m)
            .BuildCreateRequest();
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
}