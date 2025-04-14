using Microsoft.AspNetCore.Mvc;
using SplitTheBill.Api.Extensions;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.Modules.Groups.Expenses;
using SplitTheBill.Application.Modules.Groups.Payments;

namespace SplitTheBill.Api.Modules;

internal static class GroupsModule
{
    private const string GroupName = "Groups";

    internal static IEndpointRouteBuilder MapGroupsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup($"/{GroupName}")
            .WithTags(GroupName);

        group
            .MapGet("", GetGroups)
            .WithName(nameof(GetGroups))
            .ProducesOk<GetGroups.Response>();

        group
            .MapGet("{id:guid}", GetGroup)
            .WithName(nameof(GetGroup))
            .ProducesOk<GroupDto>()
            .ProducesNotFound()
            .ProducesValidationProblem();

        group
            .MapPost("", CreateGroup)
            .WithName(nameof(CreateGroup))
            .ProducesCreated<CreateGroup.Response>()
            .ProducesValidationProblem();

        group
            .MapPut("{id:guid}", UpdateGroup)
            .WithName(nameof(UpdateGroup))
            .ProducesNoContent()
            .ProducesNotFound()
            .ProducesValidationProblem();

        group
            .MapDelete("{id:guid}", DeleteGroup)
            .WithName(nameof(DeleteGroup))
            .ProducesNoContent()
            .ProducesNotFound()
            .ProducesValidationProblem();

        #region Expenses
        
        group
            .MapPost("{groupId:guid}/expenses", CreateExpense)
            .WithName(nameof(CreateExpense))
            .ProducesCreated()
            .ProducesNotFound()
            .ProducesValidationProblem();
        
        group
            .MapPut("{groupId:guid}/expenses/{expenseId:guid}", UpdateExpense)
            .WithName(nameof(UpdateExpense))
            .ProducesNoContent()
            .ProducesNotFound()
            .ProducesValidationProblem();
        
        group
            .MapDelete("{groupId:guid}/expenses/{expenseId:guid}", DeleteExpense)
            .WithName(nameof(DeleteExpense))
            .ProducesNoContent()
            .ProducesNotFound()
            .ProducesValidationProblem();

        #endregion

        #region Payments

        group
            .MapPost("{groupId:guid}/payments", CreatePayment)
            .WithName(nameof(CreatePayment))
            .ProducesCreated()
            .ProducesNotFound()
            .ProducesValidationProblem();

        group
            .MapPut("{groupId:guid}/payments/{paymentId:guid}", UpdatePayment)
            .WithName(nameof(UpdatePayment))
            .ProducesNoContent()
            .ProducesNotFound()
            .ProducesValidationProblem();

        group
            .MapDelete("{groupId:guid}/payments/{paymentId:guid}", DeletePayment)
            .WithName(nameof(DeletePayment))
            .ProducesNoContent()
            .ProducesNotFound()
            .ProducesValidationProblem();

        #endregion

        return endpoints;
    }

    private static Task<IResult> GetGroups(
        [FromServices] ISender sender
    )
        => sender.Send(new GetGroups.Request())
            .MapToOkOrProblem();

    private static Task<IResult> GetGroup(
        [FromRoute] Guid id,
        [FromServices] ISender sender
    )
        => sender.Send(new GetGroup.Request(id))
            .MapToOkOrProblem();

    private static Task<IResult> CreateGroup(
        [FromBody] CreateGroup.Request request,
        [FromServices] ISender sender
    )
        => sender.Send(request)
            .MapToCreatedOrProblem(r => $"/{GroupName}/{r.Id}");

    private static Task<IResult> UpdateGroup(
        [FromRoute] Guid id,
        [FromBody] UpdateGroup.Request request,
        [FromServices] ISender sender
    )
        => sender.Send(request with { Id = id })
            .MapToNoContentOrProblem();

    private static Task<IResult> DeleteGroup(
        [FromRoute] Guid id,
        [FromServices] ISender sender
    )
        => sender.Send(new DeleteGroup.Request(id))
            .MapToNoContentOrProblem();

    #region Expenses

    private static Task<IResult> CreateExpense(
        [FromRoute] Guid groupId,
        [FromBody] CreateExpense.Request request,
        [FromServices] ISender sender
    ) => sender.Send(request with { GroupId = groupId })
        .MapToCreatedOrProblem(_ => $"/{GroupName}/{groupId}");

    private static Task<IResult> UpdateExpense(
        [FromRoute] Guid groupId,
        [FromRoute] Guid expenseId,
        [FromBody] UpdateExpense.Request request,
        [FromServices] ISender sender
    ) => sender.Send(request with { GroupId = groupId, ExpenseId = expenseId })
        .MapToNoContentOrProblem();

    private static Task<IResult> DeleteExpense(
        [FromRoute] Guid groupId,
        [FromRoute] Guid expenseId,
        [FromServices] ISender sender
    ) => sender.Send(new DeleteExpense.Request(groupId, expenseId))
        .MapToNoContentOrProblem();

    #endregion

    #region Payments

    private static Task<IResult> CreatePayment(
        [FromRoute] Guid groupId,
        [FromBody] CreatePayment.Request request,
        [FromServices] ISender sender)
        => sender.Send(request with { GroupId = groupId })
            .MapToCreatedOrProblem(_ => $"/{GroupName}/{groupId}");

    private static Task<IResult> UpdatePayment(
        [FromRoute] Guid groupId,
        [FromRoute] Guid paymentId,
        [FromBody] UpdatePayment.Request request,
        [FromServices] ISender sender
    ) => sender.Send(request with { GroupId = groupId, PaymentId = paymentId })
        .MapToNoContentOrProblem();

    private static Task<IResult> DeletePayment(
        [FromRoute] Guid groupId,
        [FromRoute] Guid paymentId,
        [FromServices] ISender sender)
        => sender.Send(new DeletePayment.Request(groupId, paymentId))
            .MapToNoContentOrProblem();

    #endregion
}