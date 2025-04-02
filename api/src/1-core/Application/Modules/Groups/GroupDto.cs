using SplitTheBill.Domain.Models.Groups;
using SplitTheBill.Domain.Models.Members;

namespace SplitTheBill.Application.Modules.Groups;

public record GroupDto
{
    private readonly Group _group;

    public GroupDto(Group group)
    {
        _group = group;
    }

    #region Properties

    public Guid Id => _group.Id;
    public string Name => _group.Name;

    public List<MemberDto> Members => _group.Members
        .Select(m => new MemberDto(_group, m))
        .ToList();

    public List<ExpenseDto> Expenses => _group.Expenses
        .Select(e => new ExpenseDto(e))
        .ToList();

    public List<PaymentDto> Payments => _group.Payments
        .Select(p => new PaymentDto(p))
        .ToList();

    #endregion

    #region Totals

    public decimal TotalExpenseAmount => _group.Expenses.Sum(e => e.Amount);

    public decimal TotalPaymentAmount => _group.Payments.Sum(p => p.Amount);

    public decimal TotalBalance => TotalExpenseAmount - TotalPaymentAmount;

    #endregion

    #region Members

    public sealed record MemberDto
    {
        private readonly Group _group;
        private readonly Member _member;

        public MemberDto(Group group, Member member)
        {
            _group = group;
            _member = member;
        }

        public Guid Id => _member.Id;
        public string Name => _member.Name;

        #region Totals

        public decimal TotalExpenseAmount => _group.Expenses
            .Sum(e => e.GetExpenseAmountForMember(Id));

        public decimal TotalExpensePaidAmount => _group.Expenses
            .Where(e => e.PaidByMemberId == Id)
            .Sum(e => e.Amount);

        private decimal ExpenseAmountSelfPaid => _group.Expenses
            .Where(e => e.PaidByMemberId == Id && e.Participants.Any(p => p.MemberId == Id))
            .Sum(e => e.GetExpenseAmountForMember(Id));

        public decimal TotalExpenseAmountPaidByOtherMembers => _group.Expenses
            .Where(e => e.Participants.Any(p => p.MemberId == Id) && e.PaidByMemberId != Id)
            .Sum(e => e.GetExpenseAmountForMember(Id));

        public decimal TotalPaymentReceivedAmount => _group.Payments
            .Where(p => p.ReceivingMemberId == Id)
            .Sum(p => p.Amount);

        public decimal TotalPaymentSentAmount => _group.Payments
            .Where(p => p.SendingMemberId == Id)
            .Sum(p => p.Amount);

        public decimal TotalAmountOwed =>
            TotalExpensePaidAmount // total amount paid by
            - ExpenseAmountSelfPaid
            - TotalPaymentReceivedAmount; // subtract already received

        public decimal TotalAmountOwedToOtherMembers =>
            TotalExpenseAmountPaidByOtherMembers
            - TotalPaymentSentAmount; // subtract already sent

        public decimal TotalBalance => TotalAmountOwed - TotalAmountOwedToOtherMembers;

        #endregion
    }

    #endregion

    #region Expenses

    public sealed record ExpenseDto(
        Guid Id,
        string Description,
        Guid PaidByMemberId,
        DateTimeOffset Timestamp,
        decimal Amount,
        ExpenseSplitType SplitType,
        List<ExpenseDto.ExpenseParticipantDto> Participants
    )
    {
        public ExpenseDto(Expense expense) : this(
            expense.Id,
            expense.Description,
            expense.PaidByMemberId,
            expense.Timestamp,
            expense.Amount,
            expense.SplitType,
            expense.Participants
                .Select(p => new ExpenseParticipantDto(p))
                .ToList()
        )
        {
        }

        public sealed record ExpenseParticipantDto(
            Guid MemberId,
            int? PercentualShare,
            decimal? ExactShare
        )
        {
            public ExpenseParticipantDto(ExpenseParticipant expenseParticipant)
                : this(
                    expenseParticipant.MemberId,
                    expenseParticipant.PercentualShare,
                    expenseParticipant.ExactShare
                )
            {
            }
        }
    }

    #endregion

    #region Payments

    public sealed record PaymentDto(
        Guid Id,
        Guid SendingMemberId,
        Guid ReceivingMemberId,
        decimal Amount,
        DateTimeOffset Timestamp
    )
    {
        public PaymentDto(Payment payment)
            : this(
                payment.Id,
                payment.SendingMemberId,
                payment.ReceivingMemberId,
                payment.Amount,
                payment.Timestamp
            )
        {
        }
    }

    #endregion
}