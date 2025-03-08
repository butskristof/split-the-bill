using SplitTheBillPocV1.Models;

namespace SplitTheBillPocV1.Modules.Groups;

internal static class GroupsMapping
{
    internal static DetailedGroupDTO MapToDetailedGroupDTO(this Group group)
        => new(
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
}