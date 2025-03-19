using ErrorOr;
using Shouldly;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.Tests.Shared.Builders;
using SplitTheBill.Application.Tests.Shared.TestData.Builders;
using SplitTheBill.Domain.Models.Groups;
using SplitTheBill.Domain.Models.Members;

namespace SplitTheBill.Application.IntegrationTests.Modules.Groups.Payments;

internal sealed class CreatePaymentTests : ApplicationTestBase
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
            .WithGroupId(new Guid("82A68AD6-5027-4DDD-9F61-C840126B4242"))
            .WithSendingMemberId(Tests.Shared.TestData.Members.Alice.Id)
            .WithReceivingMemberId(Tests.Shared.TestData.Members.Bob.Id)
            .WithAmount(100m)
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
        Guid groupId = new("76D70E99-98D3-43AF-8C46-3C224425EE88");
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .Build()
        );
        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(groupId)
            .WithSendingMemberId(new Guid("1B3E5153-0074-4D82-864C-9AE68EEC041D"))
            .WithReceivingMemberId(Tests.Shared.TestData.Members.Bob.Id)
            .WithAmount(100m)
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
        var member = new Member
        {
            Id = new Guid("0E90B2C1-BB41-4D66-A46C-AE3E54BCFA95"),
            Name = "member not in group"
        };
        await Application.AddAsync(member);

        Guid groupId = new("76D70E99-98D3-43AF-8C46-3C224425EE88");
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .Build()
        );

        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(groupId)
            .WithSendingMemberId(member.Id)
            .WithReceivingMemberId(Tests.Shared.TestData.Members.Bob.Id)
            .WithAmount(100m)
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
        var groupId = new Guid("A6490FE4-F44C-4521-84D1-6AD0E5F88017");
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([Tests.Shared.TestData.Members.Alice.Entity()])
                .Build()
        );

        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(groupId)
            .WithSendingMemberId(Tests.Shared.TestData.Members.Alice.Id)
            .WithReceivingMemberId(new Guid("91444437-C508-45CA-874B-9C777A77E9DF"))
            .WithAmount(100m)
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
        var member = new Member
        {
            Id = new Guid("BC1DA672-E528-4C97-8956-FDA652EA8EFC"),
            Name = "member not in group"
        };
        await Application.AddAsync(member);

        var groupId = new Guid("A6490FE4-F44C-4521-84D1-6AD0E5F88017");
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([Tests.Shared.TestData.Members.Alice.Entity()])
                .Build()
        );

        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(groupId)
            .WithSendingMemberId(Tests.Shared.TestData.Members.Alice.Id)
            .WithReceivingMemberId(member.Id)
            .WithAmount(100m)
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
        Guid groupId = new("F8DE68EB-0188-4C49-9CE9-6EE72906C6BC");
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([
                    Tests.Shared.TestData.Members.Alice.Entity(),
                    Tests.Shared.TestData.Members.Bob.Entity()
                ])
                .Build()
        );

        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(groupId)
            .WithSendingMemberId(Tests.Shared.TestData.Members.Alice.Id)
            .WithReceivingMemberId(Tests.Shared.TestData.Members.Bob.Id)
            .WithAmount(100m)
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Created>();
    }

    [Test]
    public async Task ValidPaymentRequest_PersistsPayment()
    {
        Guid groupId = new("F8DE68EB-0188-4C49-9CE9-6EE72906C6BC");
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([
                    Tests.Shared.TestData.Members.Alice.Entity(),
                    Tests.Shared.TestData.Members.Bob.Entity()
                ])
                .Build()
        );

        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(groupId)
            .WithSendingMemberId(Tests.Shared.TestData.Members.Alice.Id)
            .WithReceivingMemberId(Tests.Shared.TestData.Members.Bob.Id)
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
                p => p.SendingMemberId.ShouldBe(Tests.Shared.TestData.Members.Alice.Id),
                p => p.ReceivingMemberId.ShouldBe(Tests.Shared.TestData.Members.Bob.Id),
                p => p.Amount.ShouldBe(100)
            );
    }

    [Test]
    public async Task ValidPaymentRequest_IsReturnedInGroupDtoAndUpdatesTotals()
    {
        Guid groupId = new("F8DE68EB-0188-4C49-9CE9-6EE72906C6BC");
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([
                    Tests.Shared.TestData.Members.Alice.Entity(),
                    Tests.Shared.TestData.Members.Bob.Entity()
                ])
                .Build()
        );

        var request = new CreatePaymentRequestBuilder()
            .WithGroupId(groupId)
            .WithSendingMemberId(Tests.Shared.TestData.Members.Alice.Id)
            .WithReceivingMemberId(Tests.Shared.TestData.Members.Bob.Id)
            .WithAmount(100m)
            .Build();
        await Application.SendAsync(request);

        var result = await Application.SendAsync(new GetGroup.Request(groupId));
        var response = result.Value;
        response.TotalPaymentAmount.ShouldBe(100m);
        response.Payments
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                p => p.SendingMemberId.ShouldBe(Tests.Shared.TestData.Members.Alice.Id),
                p => p.ReceivingMemberId.ShouldBe(Tests.Shared.TestData.Members.Bob.Id),
                p => p.Amount.ShouldBe(100)
            );
    }

    // TODO creating member not in group
}