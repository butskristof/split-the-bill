using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.Modules.Groups.Expenses;

public static class CreateExpense
{
    public sealed record Request : IRequest<ErrorOr<Response>>
    {
        public Guid? GroupId { get; init; }
        public string? Description { get; init; }
        public Guid? PaidByMemberId { get; init; }
        public decimal? Amount { get; init; }
        public ExpenseSplitType? SplitType { get; init; } = ExpenseSplitType.Evenly;
        public IReadOnlyCollection<Participant?> Participants { get; init; } = [];

        public sealed record Participant
        {
            public Guid? MemberId { get; init; }
            public int? PercentualShare { get; init; }
            public decimal? ExactShare { get; init; }
        }
    }

    public sealed record Response(Guid Id);

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
            RuleFor(r => r.Amount)
                .NotNullWithErrorCode()
                .PositiveDecimal(false);
            RuleFor(r => r.SplitType)
                .NotNullWithErrorCode()
                .IsInEnum()
                .WithMessage(ErrorCodes.Invalid);

            RuleFor(r => r.Participants)
                .NotEmpty()
                .WithMessage(ErrorCodes.Required);

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

    internal sealed class Handler : IRequestHandler<Request, ErrorOr<Response>>
    {
        #region construction

        private readonly ILogger<Handler> _logger;

        public Handler(ILogger<Handler> logger)
        {
            _logger = logger;
        }

        #endregion

        public Task<ErrorOr<Response>> Handle(Request request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}