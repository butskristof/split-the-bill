using ErrorOr;
using Shouldly;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.Modules.Groups.Expenses;
using SplitTheBill.Application.Tests.Shared.Builders;
using SplitTheBill.Application.Tests.Shared.TestData;
using SplitTheBill.Application.Tests.Shared.TestData.Builders;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.IntegrationTests.Modules.Groups.Expenses;

internal sealed class CreateExpenseTests() : ApplicationTestBase(true)
{
    [Test]
    public async Task InvalidRequest_ReturnsValidationErrors()
    {
        var request = new CreateExpenseRequestBuilder()
            .WithGroupId(Guid.Empty)
            .WithPaidByMemberId(null)
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList.ShouldNotBeEmpty();
        result.ErrorsOrEmptyList
            .ShouldContain(r =>
                r.Type == ErrorType.Validation &&
                r.Code == nameof(request.GroupId) &&
                r.Description == ErrorCodes.Invalid);
        result.ErrorsOrEmptyList
            .ShouldContain(r =>
                r.Type == ErrorType.Validation &&
                r.Code == nameof(request.PaidByMemberId) &&
                r.Description == ErrorCodes.Required);
    }

    [Test]
    public async Task GroupDoesNotExist_ReturnsNotFoundError()
    {
        var request = new CreateExpenseRequestBuilder()
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
    public async Task PaidByMemberDoesNotExist_ReturnsNotFoundError()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .Build()
        );
        var request = new CreateExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithPaidByMemberId(Guid.NewGuid())
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.PaidByMemberId))
            );
    }

    [Test]
    public async Task PaidByMemberNotInGroup_ReturnsNotFoundError()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .Build()
        );

        var request = new CreateExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(request.PaidByMemberId))
            );
    }

    [Test]
    public async Task ParticipantMemberDoesNotExist_ReturnsNotFoundError()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id])
                .Build()
        );
        var request = new CreateExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .WithParticipants([
                new CreateExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(Guid.NewGuid())
            ])
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(
                    $"{nameof(request.Participants)}[0].{nameof(CreateExpense.Request.Participant.MemberId)}"
                )
            );
    }

    [Test]
    public async Task ParticipantMemberNotInGroup_ReturnsNotFoundError()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id])
                .Build()
        );
        var request = new CreateExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .WithParticipants([
                new CreateExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Bob.Id)
            ])
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(
                    $"{nameof(request.Participants)}[0].{nameof(CreateExpense.Request.Participant.MemberId)}"
                )
            );
    }

    // even split
    [Test]
    public async Task ValidExpenseRequest_SplitTypeEvenly()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id, TestMembers.Bob.Id])
                .Build()
        );

        const string description = "Some fancy expense";
        const decimal amount = 200.00m;
        var request = new CreateExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithDescription(description)
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .WithAmount(amount)
            .WithSplitType(ExpenseSplitType.Evenly)
            .WithParticipants([
                new CreateExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Alice.Id),
                new CreateExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Bob.Id)
            ])
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Created>();

        var group = await Application
            .FindAsync<Group>(g => g.Id == groupId,
                g => g.Expenses);
        group!.Expenses
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe(description),
                e => e.Amount.ShouldBe(amount),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Alice.Id),
                e => e.SplitType.ShouldBe(ExpenseSplitType.Evenly),
                e => e.Participants.Count.ShouldBe(2),
                e => e.Participants.ShouldAllBe(p => p.PercentualShare == null && p.ExactShare == null),
                e => e.Participants.ShouldContain(p => p.MemberId == TestMembers.Alice.Id),
                e => e.Participants.ShouldContain(p => p.MemberId == TestMembers.Bob.Id)
            );

        var groupDto = await Application.SendAsync(new GetGroup.Request(groupId));
        var response = groupDto.Value;
        response.TotalExpenseAmount.ShouldBe(amount);
        response.Expenses
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe(description),
                e => e.Amount.ShouldBe(amount),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Alice.Id),
                e => e.SplitType.ShouldBe(ExpenseSplitType.Evenly),
                e => e.Participants.Count.ShouldBe(2),
                e => e.Participants.ShouldAllBe(p => p.PercentualShare == null && p.ExactShare == null),
                e => e.Participants.ShouldContain(p => p.MemberId == TestMembers.Alice.Id),
                e => e.Participants.ShouldContain(p => p.MemberId == TestMembers.Bob.Id)
            );
    }

    [Test]
    public async Task ValidExpenseRequest_SplitTypePercentual()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id, TestMembers.Bob.Id])
                .Build()
        );

        const string description = "Some fancy expense";
        const decimal amount = 200.00m;
        var request = new CreateExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithDescription(description)
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .WithAmount(amount)
            .WithSplitType(ExpenseSplitType.Percentual)
            .WithParticipants([
                new CreateExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Alice.Id)
                    .WithPercentualShare(60),
                new CreateExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Bob.Id)
                    .WithPercentualShare(40)
            ])
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Created>();

        var group = await Application
            .FindAsync<Group>(g => g.Id == groupId,
                g => g.Expenses);
        group!.Expenses
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe(description),
                e => e.Amount.ShouldBe(amount),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Alice.Id),
                e => e.SplitType.ShouldBe(ExpenseSplitType.Percentual),
                e => e.Participants.Count.ShouldBe(2),
                e => e.Participants.ShouldAllBe(p => p.ExactShare == null),
                e => e.Participants
                    .ShouldContain(p => p.MemberId == TestMembers.Alice.Id && p.PercentualShare == 60),
                e => e.Participants
                    .ShouldContain(p => p.MemberId == TestMembers.Bob.Id && p.PercentualShare == 40)
            );

        var groupDto = await Application.SendAsync(new GetGroup.Request(groupId));
        var response = groupDto.Value;
        response.TotalExpenseAmount.ShouldBe(amount);
        response.Expenses
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe(description),
                e => e.Amount.ShouldBe(amount),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Alice.Id),
                e => e.SplitType.ShouldBe(ExpenseSplitType.Percentual),
                e => e.Participants.Count.ShouldBe(2),
                e => e.Participants.ShouldAllBe(p => p.ExactShare == null),
                e => e.Participants
                    .ShouldContain(p => p.MemberId == TestMembers.Alice.Id && p.PercentualShare == 60),
                e => e.Participants
                    .ShouldContain(p => p.MemberId == TestMembers.Bob.Id && p.PercentualShare == 40)
            );
    }

    [Test]
    public async Task ValidExpenseRequest_SplitTypeExactAmount()
    {
        var groupId = Guid.NewGuid();
        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithMembers([TestMembers.Alice.Id, TestMembers.Bob.Id])
                .Build()
        );

        const string description = "Some fancy expense";
        const decimal amount = 200.00m;
        var request = new CreateExpenseRequestBuilder()
            .WithGroupId(groupId)
            .WithDescription(description)
            .WithPaidByMemberId(TestMembers.Alice.Id)
            .WithAmount(amount)
            .WithSplitType(ExpenseSplitType.ExactAmount)
            .WithParticipants([
                new CreateExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Alice.Id)
                    .WithExactShare(120),
                new CreateExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(TestMembers.Bob.Id)
                    .WithExactShare(80)
            ])
            .Build();
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<Created>();

        var group = await Application
            .FindAsync<Group>(g => g.Id == groupId,
                g => g.Expenses);
        group!.Expenses
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe(description),
                e => e.Amount.ShouldBe(amount),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Alice.Id),
                e => e.SplitType.ShouldBe(ExpenseSplitType.ExactAmount),
                e => e.Participants.Count.ShouldBe(2),
                e => e.Participants.ShouldAllBe(p => p.PercentualShare == null),
                e => e.Participants
                    .ShouldContain(p => p.MemberId == TestMembers.Alice.Id && p.ExactShare == 120),
                e => e.Participants
                    .ShouldContain(p => p.MemberId == TestMembers.Bob.Id && p.ExactShare == 80)
            );

        var groupDto = await Application.SendAsync(new GetGroup.Request(groupId));
        var response = groupDto.Value;
        response.TotalExpenseAmount.ShouldBe(amount);
        response.Expenses
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Description.ShouldBe(description),
                e => e.Amount.ShouldBe(amount),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Alice.Id),
                e => e.SplitType.ShouldBe(ExpenseSplitType.ExactAmount),
                e => e.Participants.Count.ShouldBe(2),
                e => e.Participants.ShouldAllBe(p => p.PercentualShare == null),
                e => e.Participants
                    .ShouldContain(p => p.MemberId == TestMembers.Alice.Id && p.ExactShare == 120),
                e => e.Participants
                    .ShouldContain(p => p.MemberId == TestMembers.Bob.Id && p.ExactShare == 80)
            );
    }


    // TODO creating member not in group
}