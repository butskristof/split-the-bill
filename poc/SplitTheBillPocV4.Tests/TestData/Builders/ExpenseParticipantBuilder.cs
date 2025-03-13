using SplitTheBillPocV4.Models;

namespace SplitTheBillPocV4.Tests.TestData.Builders;

internal class ExpenseParticipantBuilder
{
    private Guid _memberId = Guid.Empty;
    private Guid _expenseId = Guid.Empty;
    private int? _percentualShare = null;
    private decimal? _exactAmountShare = null;

    internal ExpenseParticipantBuilder WithMemberId(Guid memberId)
    {
        _memberId = memberId;
        return this;
    }

    internal ExpenseParticipantBuilder WithExpenseId(Guid expenseId)
    {
        _expenseId = expenseId;
        return this;
    }

    internal ExpenseParticipantBuilder WithPercentualSplitShare(int? percentualShare)
    {
        _percentualShare = percentualShare;
        return this;
    }

    internal ExpenseParticipantBuilder WithExactAmountSplitShare(decimal? exactShare)
    {
        _exactAmountShare = exactShare;
        return this;
    }

    internal ExpenseParticipant Build() => new()
    {
        MemberId = _memberId,
        ExpenseId = _expenseId,
        PercentualShare = _percentualShare,
        ExactAmountShare = _exactAmountShare
    };

    public static implicit operator ExpenseParticipant(ExpenseParticipantBuilder builder) => builder.Build();
}