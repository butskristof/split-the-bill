using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplitTheBillPoc.Data;

namespace SplitTheBillPoc.Modules.Members;

internal static class MembersModule
{
    internal static IEndpointRouteBuilder MapMembersModule(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/members", GetMembers);
        return builder;
    }

    private static async Task<IResult> GetMembers([FromServices] AppDbContext dbContext)
        => Results.Ok(await dbContext.Members.ToListAsync());
}