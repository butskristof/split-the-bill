using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.Tests.Shared.TestData.Builders;

public sealed class ExpenseParticipantBuilder
{
    private Guid _expenseId;
    private Guid _memberId;

    private int? _percentualShare = null;
    private decimal? _exactShare = null;

    public ExpenseParticipantBuilder WithExpenseId(Guid expenseId)
    {
        _expenseId = expenseId;
        return this;
    }

    public ExpenseParticipantBuilder WithMemberId(Guid memberId)
    {
        _memberId = memberId;
        return this;
    }

    public ExpenseParticipantBuilder WithPercentualShare(int? percentualShare)
    {
        _percentualShare = percentualShare;
        return this;
    }

    public ExpenseParticipantBuilder WithExactShare(decimal? exactShare)
    {
        _exactShare = exactShare;
        return this;
    }

    public ExpenseParticipant Build() =>
        new()
        {
            ExpenseId = _expenseId,
            MemberId = _memberId,
            PercentualShare = _percentualShare,
            ExactShare = _exactShare,
        };

    public static implicit operator ExpenseParticipant(ExpenseParticipantBuilder builder) =>
        builder.Build();
}
