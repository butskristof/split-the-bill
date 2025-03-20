using ErrorOr;
using Shouldly;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.Tests.Shared.TestData;
using SplitTheBill.Application.Tests.Shared.TestData.Builders;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.IntegrationTests.Modules.Groups;

internal sealed class GetGroupTests : ApplicationTestBase
{
    [Test]
    public async Task InvalidRequest_ReturnsValidationError()
    {
        var request = new GetGroup.Request(Guid.Empty);
        var result = await Application.SendAsync(request);

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.Validation),
                e => e.Code.ShouldBe(nameof(request.Id)),
                e => e.Description.ShouldBe(ErrorCodes.Invalid)
            );
    }

    [Test]
    public async Task GroupDoesNotExist_ReturnsNotFoundError()
    {
        var id = Guid.NewGuid();
        var result = await Application.SendAsync(new GetGroup.Request(id));

        result.IsError.ShouldBeTrue();
        result.ErrorsOrEmptyList
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Type.ShouldBe(ErrorType.NotFound),
                e => e.Code.ShouldBe(nameof(Group.Id))
            );
    }

    [Test]
    public async Task ReturnsMappedEntity()
    {
        var groupId = Guid.NewGuid();
        const string groupName = "group name";

        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithName(groupName)
                .WithMembers([
                    TestMembers.Alice,
                    TestMembers.Bob
                ])
                .WithExpenses([
                    new ExpenseBuilder()
                        .WithAmount(100)
                        .WithPaidByMemberId(TestMembers.Alice.Id)
                        .WithParticipants([
                            TestMembers.Alice,
                            TestMembers.Bob
                        ])
                ])
                .AddPayment(new PaymentBuilder()
                    .WithSendingMemberId(TestMembers.Bob.Id)
                    .WithReceivingMemberId(TestMembers.Alice.Id)
                    .WithAmount(100)
                )
                .Build()
        );

        var result = await Application.SendAsync(new GetGroup.Request(groupId));

        result.IsError.ShouldBeFalse();
        var response = result.Value;
        response.ShouldNotBeNull();
        response.Id.ShouldBe(groupId);
        response.Name.ShouldBe(groupName);

        response.TotalExpenseAmount.ShouldBe(100);

        // the goal here is to verify whether all related data is present (navigation properties,
        // children, ...) and whether the properties based on related data have a (correct) value
        // indicating all the necessary data was loaded to make the calculations
        // in-depth testing of the mapping itself is done in the unit tests
        response.Members.Count.ShouldBe(2);
        response.Members
            .ShouldContain(m => m.Id == TestMembers.Alice.Id);
        response.Members
            .ShouldContain(m => m.Id == TestMembers.Bob.Id);

        response.Expenses
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Amount.ShouldBe(100),
                e => e.PaidByMemberId.ShouldBe(TestMembers.Alice.Id),
                e => e.Participants.Count.ShouldBe(2)
            );

        response.Payments
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                p => p.Amount.ShouldBe(100),
                p => p.SendingMemberId.ShouldBe(TestMembers.Bob.Id),
                p => p.ReceivingMemberId.ShouldBe(TestMembers.Alice.Id)
            );
    }
}