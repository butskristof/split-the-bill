using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SplitTheBill.Application.Common.Persistence;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.Modules.Groups.Expenses;

public static class CreateExpense
{
    public sealed record Request : IRequest<ErrorOr<Created>>
    {
        public Guid? GroupId { get; init; }
        public string? Description { get; init; }
        public Guid? PaidByMemberId { get; init; }
        public DateTimeOffset? Timestamp { get; init; }
        public decimal? Amount { get; init; }
        public ExpenseSplitType? SplitType { get; init; } = ExpenseSplitType.Evenly;
        public IReadOnlyList<Participant?> Participants { get; init; } = [];

        public sealed record Participant
        {
            public Guid? MemberId { get; init; }
            public int? PercentualShare { get; init; }
            public decimal? ExactShare { get; init; }
        }
    }

    internal sealed class Validator : BaseValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.GroupId)
                .NotNullOrEmptyWithErrorCode();
            RuleFor(r => r.Description)
                .ValidString(true);
            RuleFor(r => r.PaidByMemberId)
                .NotNullOrEmptyWithErrorCode();
            RuleFor(r => r.Timestamp)
                .NotNullWithErrorCode();
            RuleFor(r => r.Amount)
                .NotNullWithErrorCode()
                .PositiveDecimal(false);
            RuleFor(r => r.SplitType)
                .NotNullWithErrorCode()
                .IsInEnum()
                .WithMessage(ErrorCodes.Invalid);

            RuleFor(r => r.Participants)
                .NotEmpty()
                .WithMessage(ErrorCodes.Required)
                .Must(participants =>
                    participants.Count == 1 ||
                    participants.DistinctBy(p => p?.MemberId).Count() == participants.Count
                )
                .WithMessage(ErrorCodes.NotUnique);

            RuleForEach(r => r.Participants)
                .NotNullWithErrorCode(ErrorCodes.Invalid)
                .ChildRules(v =>
                {
                    v.RuleFor(p => p!.MemberId)
                        .NotNullOrEmptyWithErrorCode();
                });

            When(r => r.SplitType == ExpenseSplitType.Evenly, () =>
            {
                RuleForEach(r => r.Participants)
                    .ChildRules(v =>
                    {
                        v.RuleFor(p => p!.PercentualShare)
                            .Null()
                            .WithMessage(ErrorCodes.Invalid);
                        v.RuleFor(p => p!.ExactShare)
                            .Null()
                            .WithMessage(ErrorCodes.Invalid);
                    });
            });

            When(r => r.SplitType == ExpenseSplitType.Percentual, () =>
                {
                    RuleForEach(r => r.Participants)
                        .ChildRules(v =>
                        {
                            v.RuleFor(p => p!.PercentualShare)
                                .NotNullWithErrorCode(ErrorCodes.Required)
                                .PositiveInteger(true);
                        });
                    RuleFor(r => r.Participants)
                        .Must(c => c.Sum(p => p!.PercentualShare) == 100)
                        .WithMessage(ErrorCodes.Invalid);
                })
                .Otherwise(() =>
                {
                    RuleForEach(r => r.Participants)
                        .ChildRules(v =>
                        {
                            v.RuleFor(p => p!.PercentualShare)
                                .Null()
                                .WithMessage(ErrorCodes.Invalid);
                        });
                });

            When(r => r.SplitType == ExpenseSplitType.ExactAmount, () =>
                {
                    RuleForEach(r => r.Participants)
                        .ChildRules(v =>
                        {
                            v.RuleFor(p => p!.ExactShare)
                                .NotNullWithErrorCode(ErrorCodes.Required)
                                .PositiveDecimal(true);
                        });
                    RuleFor(r => r.Participants)
                        .Must((rr, c) => c.Sum(p => p!.ExactShare) == rr.Amount)
                        .WithMessage(ErrorCodes.Invalid);
                })
                .Otherwise(() =>
                {
                    RuleForEach(r => r.Participants)
                        .ChildRules(v =>
                        {
                            v.RuleFor(p => p!.ExactShare)
                                .Null()
                                .WithMessage(ErrorCodes.Invalid);
                        });
                });
        }
    }

    internal sealed class Handler : IRequestHandler<Request, ErrorOr<Created>>
    {
        #region construction

        private readonly ILogger<Handler> _logger;
        private readonly IAppDbContext _dbContext;

        public Handler(ILogger<Handler> logger, IAppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        #endregion

        public async Task<ErrorOr<Created>> Handle(Request request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Adding new Expense to Group with id {GroupId}", request.GroupId);

            var group = await _dbContext
                .CurrentUserGroups(true)
                .Include(g => g.Expenses)
                .Include(g => g.Members)
                .SingleOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);
            if (group is null)
            {
                _logger.LogDebug("No group with id {Id} found in database", request.GroupId);
                return Error.NotFound(nameof(request.GroupId), $"Could not find group with id {request.GroupId}");
            }

            _logger.LogDebug("Fetched Group to add Expense to from database with related navigation properties");

            if (group.Members.All(m => m.Id != request.PaidByMemberId))
            {
                _logger.LogDebug("No member found in group with id {Id}", request.PaidByMemberId);
                return Error.NotFound(nameof(request.PaidByMemberId),
                    $"Could not find member with id {request.PaidByMemberId} in group with id {request.GroupId}");
            }

            for (var i = 0; i < request.Participants.Count; ++i)
            {
                var participant = request.Participants[i];
                if (group.Members.All(m => m.Id != participant!.MemberId))
                {
                    _logger.LogDebug("No member found in group with id {Id}", participant!.MemberId);
                    return Error.NotFound($"{nameof(request.Participants)}[{i}].{nameof(participant.MemberId)}",
                        $"Could not find member with id {participant.MemberId} in group with id {request.GroupId}");
                }
            }

            _logger.LogDebug("Verified existence of members related to expense in group");

            // request values are non-null confirmed by validator
            var expense = new Expense
            {
                Description = request.Description!,
                PaidByMemberId = request.PaidByMemberId!.Value,
                Timestamp = request.Timestamp!.Value,
            };
            switch (request.SplitType)
            {
                case ExpenseSplitType.Evenly:
                    expense.SetAmountAndParticipantsWithEvenSplit(
                        request.Amount!.Value,
                        request.Participants.Select(p => p!.MemberId!.Value).ToHashSet()
                    );
                    break;
                case ExpenseSplitType.Percentual:
                    expense.SetAmountAndParticipantsWithPercentualSplit(
                        request.Amount!.Value,
                        request.Participants.ToDictionary(p => p!.MemberId!.Value, p => p!.PercentualShare!.Value)
                    );
                    break;
                case ExpenseSplitType.ExactAmount:
                    expense.SetAmountAndParticipantsWithExactSplit(
                        request.Amount!.Value,
                        request.Participants.ToDictionary(p => p!.MemberId!.Value, p => p!.ExactShare!.Value)
                    );
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(request.SplitType));
            }

            group.Expenses.Add(expense);
            _logger.LogDebug("Mapped request to entity and added to Group's Expenses collection");

            await _dbContext.SaveChangesAsync(CancellationToken.None);
            _logger.LogDebug("Persisted changes to database");

            return Result.Created;
        }
    }
}