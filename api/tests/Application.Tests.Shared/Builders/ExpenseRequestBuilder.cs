using SplitTheBill.Application.Modules.Groups.Expenses;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.Tests.Shared.Builders;

public sealed class ExpenseRequestBuilder
{
    private Guid? _groupId = Guid.NewGuid();
    private Guid? _expenseId = Guid.NewGuid();
    private string? _description = "Test description";
    private Guid? _paidByMemberId = Guid.NewGuid();
    private DateTimeOffset? _timestamp = DateTimeOffset.UtcNow;
    private decimal? _amount = 100.00m;
    private ExpenseSplitType? _splitType = ExpenseSplitType.Evenly;

    private IReadOnlyList<CreateExpense.Request.Participant> _createParticipants =
    [
        new ParticipantBuilder().BuildCreateParticipant(),
    ];

    private IReadOnlyList<UpdateExpense.Request.Participant> _updateParticipants =
    [
        new ParticipantBuilder().BuildUpdateParticipant(),
    ];

    public ExpenseRequestBuilder WithGroupId(Guid? groupId)
    {
        _groupId = groupId;
        return this;
    }

    public ExpenseRequestBuilder WithExpenseId(Guid? expenseId)
    {
        _expenseId = expenseId;
        return this;
    }

    public ExpenseRequestBuilder WithDescription(string? description)
    {
        _description = description;
        return this;
    }

    public ExpenseRequestBuilder WithPaidByMemberId(Guid? paidByMemberId)
    {
        _paidByMemberId = paidByMemberId;
        return this;
    }

    public ExpenseRequestBuilder WithTimestamp(DateTimeOffset? timestamp)
    {
        _timestamp = timestamp;
        return this;
    }

    public ExpenseRequestBuilder WithAmount(decimal? amount)
    {
        _amount = amount;
        return this;
    }

    public ExpenseRequestBuilder WithSplitType(ExpenseSplitType? splitType)
    {
        _splitType = splitType;
        return this;
    }

    public ExpenseRequestBuilder WithParticipants(
        IReadOnlyList<CreateExpense.Request.Participant> participants
    )
    {
        _createParticipants = participants;
        return this;
    }

    public ExpenseRequestBuilder WithParticipants(
        IReadOnlyList<UpdateExpense.Request.Participant> participants
    )
    {
        _updateParticipants = participants;
        return this;
    }

    public CreateExpense.Request BuildCreateRequest() =>
        new()
        {
            GroupId = _groupId,
            Description = _description,
            PaidByMemberId = _paidByMemberId,
            Timestamp = _timestamp,
            Amount = _amount,
            SplitType = _splitType,
            Participants = _createParticipants,
        };

    public static implicit operator CreateExpense.Request(ExpenseRequestBuilder builder) =>
        builder.BuildCreateRequest();

    public UpdateExpense.Request BuildUpdateRequest() =>
        new()
        {
            GroupId = _groupId,
            ExpenseId = _expenseId,
            Description = _description,
            PaidByMemberId = _paidByMemberId,
            Timestamp = _timestamp,
            Amount = _amount,
            SplitType = _splitType,
            Participants = _updateParticipants,
        };

    public static implicit operator UpdateExpense.Request(ExpenseRequestBuilder builder) =>
        builder.BuildUpdateRequest();

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

        public CreateExpense.Request.Participant BuildCreateParticipant() =>
            new()
            {
                MemberId = _memberId,
                PercentualShare = _percentualShare,
                ExactShare = _exactShare,
            };

        public static implicit operator CreateExpense.Request.Participant(
            ParticipantBuilder builder
        ) => builder.BuildCreateParticipant();

        public UpdateExpense.Request.Participant BuildUpdateParticipant() =>
            new()
            {
                MemberId = _memberId,
                PercentualShare = _percentualShare,
                ExactShare = _exactShare,
            };

        public static implicit operator UpdateExpense.Request.Participant(
            ParticipantBuilder builder
        ) => builder.BuildUpdateParticipant();
    }
}
