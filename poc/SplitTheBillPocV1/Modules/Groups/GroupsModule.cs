using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplitTheBillPocV1.Data;

namespace SplitTheBillPocV1.Modules.Groups;

internal static class GroupsModule
{
    internal static IEndpointRouteBuilder MapGroupsModule(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("groups");

        group.MapGet("", GetGroups);
        group.MapGet("{id:guid}", MapGroup);

        return endpoints;
    }

    private static async Task<IResult> GetGroups([FromServices] AppDbContext dbContext)
    {
        var groups = await dbContext.Groups
            .Select(g => new GroupDTO(g.Id, g.Name))
            .ToListAsync();

        return TypedResults.Ok(groups);
    }

    private static async Task<IResult> MapGroup([FromRoute] Guid id, [FromServices] AppDbContext dbContext)
    {
        var group = await dbContext.Groups
            .Include(g => g.Members)
            .Include(g => g.Expenses)
            .Include(group => group.Payments)
            .SingleOrDefaultAsync(g => g.Id == id);
        if (group is null) return Results.NotFound();

        var mappedGroup = new DetailedGroupDTO(
            group.Id,
            group.Name,
            group.Members
                .Select(m => new DetailedGroupDTO.MemberDTO(m.Id, m.Name))
                .ToList(),
            group.Expenses
                .Select(e => new DetailedGroupDTO.ExpenseDTO(e.Id, e.Description, e.Amount))
                .ToList(),
            group.Payments
                .Select(p => new DetailedGroupDTO.PaymentDTO(p.Id, p.MemberId, p.Amount))
                .ToList()
        );
        return TypedResults.Ok(mappedGroup);
    }
}