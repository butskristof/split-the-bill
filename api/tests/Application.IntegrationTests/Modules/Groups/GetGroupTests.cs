using ErrorOr;
using Shouldly;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Modules.Groups;
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
        Guid id = new("DC6F28A5-41B2-4970-A2DD-02040F4CA593");
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
        Guid groupId = new("5AA6E512-58CC-4224-A000-C9198CE5D6F8");
        const string groupName = "group name";

        await Application.AddAsync(
            new GroupBuilder()
                .WithId(groupId)
                .WithName(groupName)
                .WithMembers([
                    Tests.Shared.TestData.Members.Alice.Entity(),
                    Tests.Shared.TestData.Members.Bob.Entity()
                ])
                .WithExpenses([
                    new ExpenseBuilder()
                        .WithAmount(100)
                        .WithPaidByMemberId(Tests.Shared.TestData.Members.Alice.Id)
                        .WithParticipants([
                            Tests.Shared.TestData.Members.Alice.Entity(),
                            Tests.Shared.TestData.Members.Bob.Entity()
                        ])
                ])
                .AddPayment(new PaymentBuilder()
                    .WithSendingMemberId(Tests.Shared.TestData.Members.Bob.Id)
                    .WithReceivingMemberId(Tests.Shared.TestData.Members.Alice.Id)
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
            .ShouldContain(m => m.Id == Tests.Shared.TestData.Members.Alice.Id);
        response.Members
            .ShouldContain(m => m.Id == Tests.Shared.TestData.Members.Bob.Id);

        response.Expenses
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                e => e.Amount.ShouldBe(100),
                e => e.PaidByMemberId.ShouldBe(Tests.Shared.TestData.Members.Alice.Id),
                e => e.Participants.Count.ShouldBe(2)
            );

        response.Payments
            .ShouldHaveSingleItem()
            .ShouldSatisfyAllConditions(
                p => p.Amount.ShouldBe(100),
                p => p.SendingMemberId.ShouldBe(Tests.Shared.TestData.Members.Bob.Id),
                p => p.ReceivingMemberId.ShouldBe(Tests.Shared.TestData.Members.Alice.Id)
            );
    }
}