using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplitTheBillPocV3.Data;

namespace SplitTheBillPocV3.Modules;

internal static class GroupsModule
{
    internal static IEndpointRouteBuilder MapGroupsModule(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("groups");

        group.MapGet("", GetGroups);
        group.MapGet("{id:guid}", GetGroup);

        return endpoints;
    }

    private static async Task<IResult> GetGroups([FromServices] AppDbContext dbContext)
    {
        var groups = await dbContext.Groups
            .Select(g => new GroupDTO(g.Id, g.Name))
            .ToListAsync();

        return TypedResults.Ok(groups);
    }

    private static async Task<IResult> GetGroup([FromRoute] Guid id, [FromServices] AppDbContext dbContext)
    {
        var group = await dbContext.Groups
            .Include(g => g.Members)
            .Include(g => g.Expenses)
            .Include(group => group.Payments)
            .SingleOrDefaultAsync(g => g.Id == id);
        if (group is null) return Results.NotFound();

        var mappedGroup = group.MapToDetailedGroupDTO();
        return TypedResults.Ok(mappedGroup);
    }
}