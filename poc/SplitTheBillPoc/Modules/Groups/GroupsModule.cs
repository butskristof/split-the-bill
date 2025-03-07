using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplitTheBillPoc.Data;

namespace SplitTheBillPoc.Modules.Groups;

internal static class GroupsModule
{
    internal static IEndpointRouteBuilder MapGroupsModule(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/groups");
        group.MapGet("", GetGroups);
        return builder;
    }

    private static async Task<IResult> GetGroups([FromServices] AppDbContext dbContext)
    {
        var groups = await dbContext.Groups
            .Include(g => g.Members)
            .Include(g => g.Expenses)
            .Include(g => g.Payments)
            .ToListAsync();
        var mappedGroups = groups
            .Select(g => g.ToDTO());
        return Results.Ok(mappedGroups);
    }
}