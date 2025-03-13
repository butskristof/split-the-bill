using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.UnitTests.TestData.Builders;

internal sealed class ExpenseParticipantBuilder
{
    private Guid _expenseId;
    private Guid _memberId;

    private int? _percentualShare = null;
    private decimal? _exactShare = null;

    internal ExpenseParticipantBuilder WithExpenseId(Guid expenseId)
    {
        _expenseId = expenseId;
        return this;
    }

    internal ExpenseParticipantBuilder WithMemberId(Guid memberId)
    {
        _memberId = memberId;
        return this;
    }

    internal ExpenseParticipantBuilder WithPercentualShare(int? percentualShare)
    {
        _percentualShare = percentualShare;
        return this;
    }

    internal ExpenseParticipantBuilder WithExactShare(decimal? exactShare)
    {
        _exactShare = exactShare;
        return this;
    }

    internal ExpenseParticipant Build() => new()
    {
        ExpenseId = _expenseId,
        MemberId = _memberId,
        PercentualShare = _percentualShare,
        ExactShare = _exactShare,
    };

    public static implicit operator ExpenseParticipant(ExpenseParticipantBuilder builder) => builder.Build();
}