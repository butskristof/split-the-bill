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
        List<ExpenseParticipantDTO> Participants);

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
                var totalExpenseAmountForMember = Expenses
                    .Sum(e =>
                    {
                        if (e.Amount == 0) return 0;
                        var participantForMember = e.Participants.SingleOrDefault(p => p.MemberId == memberId);
                        if (participantForMember == null) return 0;
                        var amount = e.SplitType switch
                        {
                            ExpenseSplitType.Evenly => e.Amount / e.Participants.Count,
                            ExpenseSplitType.Percentual => e.Amount *
                                                           (decimal)participantForMember.PercentualSplitShare.Value,
                            ExpenseSplitType.ExactAmount => participantForMember.ExactAmountSplitShare.Value,
                            _ => throw new ArgumentOutOfRangeException()
                        };
                        return amount;
                    });

                var paidAmount = Payments
                    .Where(p => p.SendingMemberId == m.Id)
                    .Sum(p => p.Amount);
                var receivedAmount = Payments
                    .Where(p => p.ReceivingMemberId == m.Id)
                    .Sum(p => p.Amount);
                return totalExpenseAmountForMember - paidAmount - receivedAmount;
            }
        );
}