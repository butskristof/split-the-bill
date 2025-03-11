using System.Collections.Immutable;
using SplitTheBillPocV4.Models;

namespace SplitTheBillPocV4.Modules;

internal sealed record EnrichedGroupModel
{
    private readonly Group _group;

    public EnrichedGroupModel(Group group)
    {
        _group = group;
    }

    public Guid Id => _group.Id;
    public string Name => _group.Name;

    #region Members

    internal sealed record EnrichedMemberModel
    {
        private readonly Group _group;
        private readonly Member _member;

        public EnrichedMemberModel(Group group, Member member)
        {
            _group = group;
            _member = member;
        }

        public Guid Id => _member.Id;
        public string Name => _member.Name;

        /// <summary>
        /// Total amount of expenses attributed to this member
        /// </summary>
        public decimal TotalExpenseAmount => _group.Expenses
            .Sum(e => e.GetExpenseAmountForMember(Id));

        /// <summary>
        /// Total amount of expenses paid by this member
        /// </summary>
        public decimal TotalExpensePaidAmount => _group.Expenses
            .Where(e => e.PaidByMemberId == Id)
            .Sum(e => e.Amount);

        /// <summary>
        /// Share of TotalExpenseAmount which is self-paid
        /// </summary>
        private decimal ExpenseAmountSelfPaid => _group.Expenses
            .Where(e => e.PaidByMemberId == Id && e.Participants.Any(p => p.MemberId == Id))
            .Sum(e => e.GetExpenseAmountForMember(Id));

        /// <summary>
        /// Share of TotalExpenseAmount lent by other members to this member
        /// </summary>
        public decimal TotalExpenseAmountPaidByOtherMembers => _group.Expenses
            .Where(e => e.Participants.Any(p => p.MemberId == Id) && e.PaidByMemberId != Id)
            .Sum(e => e.GetExpenseAmountForMember(Id));

        /// <summary>
        /// Total of payments received by this member from other members
        /// </summary>
        public decimal TotalPaymentReceivedAmount => _group.Payments
            .Where(p => p.ReceivingMemberId == Id)
            .Sum(p => p.Amount);

        /// <summary>
        /// Total of payments sent by this member to other members
        /// </summary>
        public decimal TotalPaymentSentAmount => _group.Payments
            .Where(p => p.ReceivingMemberId == Id)
            .Sum(p => p.Amount);

        /// <summary>
        /// Total amount to be received by this member from other members (what is owed TO this member)
        /// </summary>
        public decimal TotalAmountOwed =>
            TotalExpensePaidAmount // total amount paid by
            - ExpenseAmountSelfPaid
            - TotalPaymentReceivedAmount; // subtract already received

        /// <summary>
        /// Total amount to be sent by this member to other members (what is owed BY this member)
        /// </summary>
        public decimal TotalAmountOwedToOtherMembers =>
            TotalExpenseAmountPaidByOtherMembers
            - TotalPaymentSentAmount; // subtract already sent

        /// <summary>
        /// Total balance that is owed to/by this member
        /// </summary>
        public decimal Balance => TotalAmountOwed - TotalAmountOwedToOtherMembers;
    }

    public IReadOnlyList<EnrichedMemberModel> Members => _group
        .Members
        .Select(m => new EnrichedMemberModel(_group, m))
        .ToImmutableList();

    #endregion

    #region Expenses

    internal sealed record EnrichedExpenseModel
    {
        private readonly Expense _expense;

        public EnrichedExpenseModel(Expense expense)
        {
            _expense = expense;
        }
    }

    public IReadOnlyList<EnrichedExpenseModel> Expenses => _group
        .Expenses
        .Select(e => new EnrichedExpenseModel(e))
        .ToImmutableList();

    #endregion

    #region Payments

    internal sealed record EnrichedPaymentModel
    {
        private readonly Payment _payment;

        public EnrichedPaymentModel(Payment payment)
        {
            _payment = payment;
        }

        public Guid Id => _payment.Id;
        public Guid SendingMemberId => _payment.SendingMemberId;
        public Guid ReceivingMemberId => _payment.ReceivingMemberId;
        public decimal Amount => _payment.Amount;
    }

    public IReadOnlyList<EnrichedPaymentModel> Payments => _group
        .Payments
        .Select(p => new EnrichedPaymentModel(p))
        .ToImmutableList();

    #endregion

    #region Totals

    public decimal TotalExpenseAmount => _group.Expenses.Sum(e => e.Amount);
    public decimal TotalPaymentAmount => _group.Payments.Sum(p => p.Amount);
    public decimal TotalAmountDue => TotalExpenseAmount - TotalPaymentAmount;

    #endregion
}