using MediatR;
using Microsoft.AspNetCore.Mvc;
using SplitTheBill.Api.Extensions;
using SplitTheBill.Application.Modules.Groups;

namespace SplitTheBill.Api.Modules;

internal static class GroupsModule
{
    private const string GroupName = "Groups";

    internal static IEndpointRouteBuilder MapGroupsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup($"/{GroupName}");

        group
            .MapGet("", GetGroups);

        group
            .MapGet("/{id:guid}", GetGroup);

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
}