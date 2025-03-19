using MediatR;
using Microsoft.AspNetCore.Mvc;
using SplitTheBill.Api.Extensions;
using SplitTheBill.Application.Modules.Groups;
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

        group
            .MapPost("{groupId:guid}/payments", CreatePayment)
            .WithName(nameof(CreatePayment))
            .ProducesCreated()
            .ProducesNotFound()
            .ProducesValidationProblem();
        
        group
            .MapDelete("{groupId:guid}/payments/{paymentId:guid}", DeletePayment)
            .WithName(nameof(DeletePayment))
            .ProducesNoContent()
            .ProducesNotFound()
            .ProducesValidationProblem();

        return endpoints;
    }

    internal static Task<IResult> GetGroups(
        [FromServices] ISender sender
    )
        => sender.Send(new GetGroups.Request())
            .MapToOkOrProblem();

    internal static Task<IResult> GetGroup(
        [FromRoute] Guid id,
        [FromServices] ISender sender
    )
        => sender.Send(new GetGroup.Request(id))
            .MapToOkOrProblem();

    internal static Task<IResult> CreateGroup(
        [FromBody] CreateGroup.Request request,
        [FromServices] ISender sender
    )
        => sender.Send(request)
            .MapToCreatedOrProblem(r => $"/{GroupName}/{r.Id}");

    internal static Task<IResult> UpdateGroup(
        [FromRoute] Guid id,
        [FromBody] UpdateGroup.Request request,
        [FromServices] ISender sender
    )
        => sender.Send(request with { Id = id })
            .MapToNoContentOrProblem();

    internal static Task<IResult> DeleteGroup(
        [FromRoute] Guid id,
        [FromServices] ISender sender
    )
        => sender.Send(new DeleteGroup.Request(id))
            .MapToNoContentOrProblem();

    internal static Task<IResult> CreatePayment(
        [FromRoute] Guid groupId,
        [FromBody] CreatePayment.Request request,
        [FromServices] ISender sender)
        => sender.Send(request with { GroupId = groupId })
            .MapToCreatedOrProblem(r => $"/{GroupName}/{groupId}");

    internal static Task<IResult> DeletePayment(
        [FromRoute] Guid groupId,
        [FromRoute] Guid paymentId,
        [FromServices] ISender sender)
        => sender.Send(new DeletePayment.Request(groupId, paymentId))
            .MapToNoContentOrProblem();
}