using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SplitTheBill.Application.Common.Persistence;
using SplitTheBill.Application.Common.Validation;

namespace SplitTheBill.Application.Modules.Groups.Payments;

public static class DeletePayment
{
    public sealed record Request(Guid GroupId, Guid PaymentId) : ICommand<ErrorOr<Deleted>>;

    internal sealed class Validator : BaseValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.GroupId)
                .NotEmptyWithErrorCode(ErrorCodes.Invalid);
            RuleFor(r => r.PaymentId)
                .NotEmptyWithErrorCode(ErrorCodes.Invalid);
        }
    }

    internal sealed class Handler : ICommandHandler<Request, ErrorOr<Deleted>>
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

        public async ValueTask<ErrorOr<Deleted>> Handle(Request request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Deleting Payment with id {PaymentId} from group {GroupId}", 
                request.PaymentId, request.GroupId);
            
            var group = await _dbContext
                .CurrentUserGroups(true)
                .Include(g => g.Payments)
                .SingleOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);
            if (group is null)
            {
                _logger.LogDebug("No group with id {GroupId} found in database", request.GroupId);
                return Error.NotFound(nameof(request.GroupId), $"Could not find group with id {request.GroupId}");
            }
            _logger.LogDebug("Fetched group to delete payment from from database");
            
            var payment = group.Payments
                .SingleOrDefault(p => p.Id == request.PaymentId);
            if (payment is null)
            {
                _logger.LogDebug("No payment with id {PaymentId} found in group {GroupId}", 
                    request.PaymentId, request.GroupId);
                return Error.NotFound(nameof(request.PaymentId), $"Could not find payment with id {request.PaymentId} in group with id {request.GroupId}");
            }

            group.Payments.Remove(payment);
            _logger.LogDebug("Deleted payment with id {PaymentId} from group {GroupId}", 
                request.PaymentId, request.GroupId);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
            _logger.LogDebug("Persisted changes to database");
            
            return Result.Deleted;
        }
    }
}