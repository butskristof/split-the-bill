namespace SplitTheBillPoc.Modules.Groups;

internal sealed record GroupDTO(
    Guid Id,
    string Name
);

internal sealed record DetailedGroupDTO(
    Guid Id,
    string Name,
    List<DetailedGroupDTO.GroupMemberDTO> Members,
    List<DetailedGroupDTO.ExpenseDTO> Expenses
)
{
    internal sealed record GroupMemberDTO(Guid MemberId, string Name);

    internal sealed record ExpenseDTO(Guid Id, string Description, decimal Amount);
}