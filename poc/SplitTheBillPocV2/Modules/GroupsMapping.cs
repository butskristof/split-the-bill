using SplitTheBillPocV2.Models;

namespace SplitTheBillPocV2.Modules;

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
                .Select(e => new DetailedGroupDTO.ExpenseDTO(e.Id, e.Description, e.Amount,
                    e.Participants
                        .Select(p => new DetailedGroupDTO.MemberDTO(p.Id, p.Name))
                        .ToList())
                )
                .ToList(),
            group.Payments
                .Select(p => new DetailedGroupDTO.PaymentDTO(p.Id, p.MemberId, p.Amount))
                .ToList()
        );
}