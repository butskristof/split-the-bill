using SplitTheBill.Domain.Models.Groups;
using SplitTheBill.Domain.Models.Members;

namespace SplitTheBill.Application.Modules.Groups;

internal sealed record GroupDto
{
    private readonly Group _group;

    public GroupDto(Group group)
    {
        _group = group;
    }

    public Guid Id => _group.Id;
    public string Name => _group.Name;

    public List<MemberDto> Members => _group.Members
        .Select(m => new MemberDto(m))
        .ToList();

    public List<ExpenseDto> Expenses => _group.Expenses
        .Select(e => new ExpenseDto(e))
        .ToList();

    public List<PaymentDto> Payments => _group.Payments
        .Select(p => new PaymentDto(p))
        .ToList();

    #region Members

    internal sealed record MemberDto(
        Guid Id,
        string Name
    )
    {
        public MemberDto(Member member) : this(
            member.Id,
            member.Name
        )
        {
        }
    }

    #endregion

    #region Expenses

    internal sealed record ExpenseDto(
        Guid Id,
        string Description,
        decimal Amount,
        ExpenseSplitType SplitType,
        List<ExpenseDto.ExpenseParticipantDto> Participants,
        Guid PaidByMemberId
    )
    {
        public ExpenseDto(Expense expense) : this(
            expense.Id,
            expense.Description,
            expense.Amount,
            expense.SplitType,
            expense.Participants
                .Select(p => new ExpenseParticipantDto(p))
                .ToList(),
            expense.PaidByMemberId
        )
        {
        }

        internal sealed record ExpenseParticipantDto(
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

    internal sealed record PaymentDto(
        Guid Id,
        Guid SendingMemberId,
        Guid ReceivingMemberId,
        decimal Amount
    )
    {
        public PaymentDto(Payment payment)
            : this(payment.Id, payment.SendingMemberId, payment.ReceivingMemberId, payment.Amount)
        {
        }
    }

    #endregion
}