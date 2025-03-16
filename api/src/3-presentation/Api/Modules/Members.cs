using MediatR;
using Microsoft.AspNetCore.Mvc;
using SplitTheBill.Api.Extensions;
using SplitTheBill.Application.Modules.Members;

namespace SplitTheBill.Api.Modules;

internal static class Members
{
    private const string GroupName = "Members";

    internal static IEndpointRouteBuilder MapMembersEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup($"/{GroupName}");

        group
            .MapGet("", GetMembers);

        return endpoints;
    }

    internal static Task<IResult> GetMembers(
        [FromServices] ISender sender
    )
        => sender.Send(new GetMembers.Request())
            .MapToOkOrProblem();
}