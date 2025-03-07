namespace SplitTheBillPoc.Modules.Groups;

internal sealed record GroupDTO(
    Guid Id,
    string Name
);

internal sealed record DetailedGroupDTO(
    Guid Id,
    string Name,
    List<DetailedGroupDTO.MemberDTO> Members,
    List<DetailedGroupDTO.ExpenseDTO> Expenses,
    List<DetailedGroupDTO.PaymentDTO> Payments
)
{
    internal sealed record MemberDTO(Guid Id, string Name);

    internal sealed record ExpenseDTO(Guid Id, string Description, decimal Amount);

    internal sealed record PaymentDTO(Guid Id, Guid MemberId, decimal Amount);

    public decimal TotalExpenseAmount => Expenses.Sum(e => e.Amount);
    public decimal TotalPaymentAmount => Payments.Sum(p => p.Amount);
    public decimal AmountDue => TotalExpenseAmount - TotalPaymentAmount;
    public decimal ExpenseAmountPerMember => TotalExpenseAmount / Members.Count;

    public Dictionary<Guid, decimal> AmountsDueByMember => Members
        .ToDictionary(
            m => m.Id,
            m =>
                ExpenseAmountPerMember - Payments
                    .Where(p => p.MemberId == m.Id)
                    .Sum(p => p.Amount)
        );
}