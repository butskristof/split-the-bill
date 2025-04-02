using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SplitTheBill.Application.Common.Persistence;
using SplitTheBill.Application.Common.Validation;

namespace SplitTheBill.Application.Modules.Groups.Payments;

public static class UpdatePayment
{
    public sealed record Request : IRequest<ErrorOr<Updated>>
    {
        public Guid? GroupId { get; init; }
        public Guid? PaymentId { get; init; }
        public Guid? SendingMemberId { get; init; }
        public Guid? ReceivingMemberId { get; init; }
        public decimal? Amount { get; init; }
        public DateTimeOffset? Timestamp { get; init; }
    }

    internal sealed class Validator : BaseValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.GroupId)
                .NotNullOrEmptyWithErrorCode();
            RuleFor(r => r.PaymentId)
                .NotNullOrEmptyWithErrorCode();
            RuleFor(r => r.SendingMemberId)
                .NotNullOrEmptyWithErrorCode();
            RuleFor(r => r.ReceivingMemberId)
                .NotNullOrEmptyWithErrorCode()
                .NotEqual(r => r.SendingMemberId)
                .WithMessage(ErrorCodes.NotUnique);
            RuleFor(r => r.Amount)
                .NotNullWithErrorCode()
                .PositiveDecimal(false);
        }
    }

    internal sealed class Handler : IRequestHandler<Request, ErrorOr<Updated>>
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

        public async Task<ErrorOr<Updated>> Handle(Request request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Updating payment with id {PaymentId} in Group with id {GroupId}",
                request.PaymentId, request.GroupId);

            var group = await _dbContext
                .CurrentUserGroups(true)
                .Include(g => g.Payments)
                .Include(g => g.Members)
                .SingleOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);
            if (group is null)
            {
                _logger.LogDebug("No group with id {Id} found in database", request.GroupId);
                return Error.NotFound(nameof(request.GroupId), $"Could not find group with id {request.GroupId}");
            }

            _logger.LogDebug("Fetched group to update payment in from database with related navigation properties");

            var payment = group.Payments
                .SingleOrDefault(p => p.Id == request.PaymentId);
            if (payment is null)
            {
                _logger.LogDebug("No payment with id {PaymentId} found in group {GroupId}",
                    request.PaymentId, request.GroupId);
                return Error.NotFound(nameof(request.PaymentId),
                    $"Could not find payment with id {request.PaymentId} in group with id {request.GroupId}");
            }
            _logger.LogDebug("Found Payment to update in Group");

            if (group.Members.All(m => m.Id != request.SendingMemberId))
            {
                _logger.LogDebug("No member found in group with id {Id}", request.SendingMemberId);
                return Error.NotFound(nameof(request.SendingMemberId),
                    $"Could not find member with id {request.SendingMemberId} in group with id {request.GroupId}");
            }

            if (group.Members.All(m => m.Id != request.ReceivingMemberId))
            {
                _logger.LogDebug("No member found in group with id {Id}", request.ReceivingMemberId);
                return Error.NotFound(nameof(request.ReceivingMemberId),
                    $"Could not find member with id {request.ReceivingMemberId} in group with id {request.GroupId}");
            }

            _logger.LogDebug("Verified existence of sending and receiving member in group");

            // request values are non-null confirmed by validator
            payment.SendingMemberId = request.SendingMemberId!.Value;
            payment.ReceivingMemberId = request.ReceivingMemberId!.Value;
            payment.Amount = request.Amount!.Value;
            payment.Timestamp = request.Timestamp!.Value;
            _logger.LogDebug("Mapped values from request to entity");

            await _dbContext.SaveChangesAsync(CancellationToken.None);
            _logger.LogDebug("Persisted changes to database");

            return Result.Updated;
        }
    }
}