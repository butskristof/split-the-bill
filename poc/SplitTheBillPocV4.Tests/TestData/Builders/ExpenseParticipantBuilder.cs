using SplitTheBillPocV4.Models;

namespace SplitTheBillPocV4.Tests.TestData.Builders;

internal class ExpenseParticipantBuilder
{
    private Guid _memberId = Guid.Empty;
    private Guid _expenseId = Guid.Empty;
    private double? _percentualSplitShare = null;
    private decimal? _exactAmountSplitShare = null;
    
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
    
    internal ExpenseParticipantBuilder WithPercentualSplitShare(double? splitShare)
    {
        _percentualSplitShare = splitShare;
        return this;
    }
    
    internal ExpenseParticipantBuilder WithExactAmountSplitShare(decimal? exactSplit)
    {
        _exactAmountSplitShare = exactSplit;
        return this;
    }
    
    internal ExpenseParticipant Build() => new()
    {
        MemberId = _memberId,
        ExpenseId = _expenseId,
        PercentualSplitShare = _percentualSplitShare,
        ExactAmountSplitShare = _exactAmountSplitShare
    };

    public static implicit operator ExpenseParticipant(ExpenseParticipantBuilder builder) => builder.Build();
}