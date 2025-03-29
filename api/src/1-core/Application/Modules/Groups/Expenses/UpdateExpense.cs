using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.Modules.Groups.Expenses;

public static class UpdateExpense
{
    public sealed record Request : IRequest<ErrorOr<Updated>>
    {
        public Guid? GroupId { get; init; }
        public Guid? ExpenseId { get; init; }
        public string? Description { get; init; }
        public Guid? PaidByMemberId { get; init; }
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
        }
    }

    internal sealed class Handler : IRequestHandler<Request, ErrorOr<Updated>>
    {
        #region construction

        private readonly ILogger<Handler> _logger;

        public Handler(ILogger<Handler> logger)
        {
            _logger = logger;
        }

        #endregion

        public Task<ErrorOr<Updated>> Handle(Request request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}