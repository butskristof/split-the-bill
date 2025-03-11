using SplitTheBillPocV4.Models;

namespace SplitTheBillPocV4.Modules;

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

    internal sealed record ExpenseDTO(
        Guid Id,
        string Description,
        decimal Amount,
        ExpenseSplitType SplitType,
        List<ExpenseParticipantDTO> Participants,
        Guid PaidByMemberId);

    internal sealed record ExpenseParticipantDTO(
        Guid MemberId,
        double? PercentualSplitShare,
        decimal? ExactAmountSplitShare);

    internal sealed record PaymentDTO(Guid Id, Guid SendingMemberId, Guid ReceivingMemberId, decimal Amount);

    public decimal TotalExpenseAmount => Expenses.Sum(e => e.Amount);
    public decimal TotalPaymentAmount => Payments.Sum(p => p.Amount);
    public decimal AmountDue => TotalExpenseAmount - TotalPaymentAmount;

    public Dictionary<Guid, decimal> AmountsDueByMember => Members
        .ToDictionary(
            m => m.Id,
            m =>
            {
                var memberId = m.Id;
                return -1m;
            }
        );

    public Dictionary<Guid, Dictionary<Guid, decimal>> BalancesByMember => Members
        .ToDictionary(
            m => m.Id,
            m =>
            {
                return new Dictionary<Guid, decimal>();
            });
}