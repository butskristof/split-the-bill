namespace SplitTheBillPoc.Modules.Groups;

internal sealed record GroupDTO(
    Guid Id,
    string Name
);

internal sealed record DetailedGroupDTO(
    Guid Id,
    string Name,
    List<DetailedGroupDTO.GroupMemberDTO> Members,
    List<DetailedGroupDTO.ExpenseDTO> Expenses,
    List<DetailedGroupDTO.PaymentDTO> Payments
)
{
    internal sealed record GroupMemberDTO(Guid MemberId, string Name);

    internal sealed record ExpenseDTO(Guid Id, string Description, decimal Amount);
    
    internal sealed record PaymentDTO(Guid Id, decimal Amount);
    
    public decimal TotalExpenseAmount => Expenses.Sum(e => e.Amount);
    public decimal TotalPaymentAmount => Payments.Sum(p => p.Amount);
    public decimal AmountDue => TotalExpenseAmount - TotalPaymentAmount;
}