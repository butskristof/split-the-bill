using Microsoft.AspNetCore.Mvc;
using SplitTheBill.Api.Extensions;
using SplitTheBill.Application.Modules.Members;

namespace SplitTheBill.Api.Modules;

internal static class MembersModule
{
    private const string GroupName = "Members";

    internal static IEndpointRouteBuilder MapMembersEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup($"/{GroupName}")
            .WithTags(GroupName);

        group
            .MapGet("", GetMembers)
            .WithName(nameof(GetMembers))
            .ProducesOk<GetMembers.Response>();

        return endpoints;
    }

    private static Task<IResult> GetMembers(
        [FromServices] ISender sender
    )
        => sender.Send(new GetMembers.Request())
            .MapToOkOrProblem();
}