using SplitTheBill.Application.Modules.Groups.Expenses;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.Tests.Shared.Builders;

public sealed class CreateExpenseRequestBuilder
{
    private Guid? _groupId = Guid.NewGuid();
    private string? _description = "Test description";
    private decimal? _amount = 100.00m;
    private Guid? _paidByMemberId = Guid.NewGuid();
    private ExpenseSplitType? _splitType = ExpenseSplitType.Evenly;
    private IReadOnlyCollection<CreateExpense.Request.Participant> _participants = [];

    public CreateExpenseRequestBuilder WithGroupId(Guid? groupId)
    {
        _groupId = groupId;
        return this;
    }
    
    public CreateExpenseRequestBuilder WithDescription(string? description)
    {
        _description = description;
        return this;
    }

    public CreateExpenseRequestBuilder WithPaidByMemberId(Guid? paidByMemberId)
    {
        _paidByMemberId = paidByMemberId;
        return this;
    }
    
    public CreateExpenseRequestBuilder WithAmount(decimal? amount)
    {
        _amount = amount;
        return this;
    }

    public CreateExpenseRequestBuilder WithSplitType(ExpenseSplitType? splitType)
    {
        _splitType = splitType;
        return this;
    }

    public CreateExpenseRequestBuilder WithParticipants(IReadOnlyCollection<CreateExpense.Request.Participant> participants)
    {
        _participants = participants;
        return this;
    }

    public CreateExpense.Request Build() => new()
    {
        GroupId = _groupId,
        Description = _description,
        PaidByMemberId = _paidByMemberId,
        Amount = _amount,
        SplitType = _splitType,
        Participants = _participants,
    };

    public static implicit operator CreateExpense.Request(CreateExpenseRequestBuilder builder) => builder.Build();

    public sealed class ParticipantBuilder
    {
        private Guid? _memberId = Guid.NewGuid();
        private int? _percentualShare = null;
        private decimal? _exactShare = null;

        public ParticipantBuilder WithMemberId(Guid? memberId)
        {
            _memberId = memberId;
            return this;
        }

        public ParticipantBuilder WithPercentualShare(int? percentualShare)
        {
            _percentualShare = percentualShare;
            return this;
        }

        public ParticipantBuilder WithExactShare(decimal? exactShare)
        {
            _exactShare = exactShare;
            return this;
        }

        public CreateExpense.Request.Participant Build() => new()
        {
            MemberId = _memberId,
            PercentualShare = _percentualShare,
            ExactShare = _exactShare
        };

        public static implicit operator CreateExpense.Request.Participant(ParticipantBuilder builder) => builder.Build();
    }
}
