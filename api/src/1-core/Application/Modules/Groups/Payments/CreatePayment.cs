using ErrorOr;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SplitTheBill.Application.Common.Persistence;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.Modules.Groups.Payments;

public static class CreatePayment
{
    public sealed record Request : ICommand<ErrorOr<Created>>
    {
        public Guid? GroupId { get; init; }
        public Guid? SendingMemberId { get; init; }
        public Guid? ReceivingMemberId { get; init; }
        public decimal? Amount { get; init; }
        public DateTimeOffset? Timestamp { get; init; }
    }

    internal sealed class Validator : BaseValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.GroupId).NotNullOrEmptyWithErrorCode();
            RuleFor(r => r.SendingMemberId).NotNullOrEmptyWithErrorCode();
            RuleFor(r => r.ReceivingMemberId)
                .NotNullOrEmptyWithErrorCode()
                .NotEqual(r => r.SendingMemberId)
                .WithMessage(ErrorCodes.NotUnique);
            RuleFor(r => r.Amount).NotNullWithErrorCode().PositiveDecimal(false);
            RuleFor(r => r.Timestamp).NotNullWithErrorCode();
        }
    }

    internal sealed class Handler : ICommandHandler<Request, ErrorOr<Created>>
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

        public async ValueTask<ErrorOr<Created>> Handle(
            Request request,
            CancellationToken cancellationToken
        )
        {
            _logger.LogDebug("Adding new Payment to Group with id {GroupId}", request.GroupId);

            var group = await _dbContext
                .CurrentUserGroups(true)
                .Include(g => g.Payments)
                .Include(g => g.Members)
                .SingleOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);
            if (group is null)
            {
                _logger.LogDebug("No group with id {Id} found in database", request.GroupId);
                return Error.NotFound(
                    nameof(request.GroupId),
                    $"Could not find group with id {request.GroupId}"
                );
            }

            _logger.LogDebug(
                "Fetched Group to add Payment to from database with related navigation properties"
            );

            if (group.Members.All(m => m.Id != request.SendingMemberId))
            {
                _logger.LogDebug("No member found in group with id {Id}", request.SendingMemberId);
                return Error.NotFound(
                    nameof(request.SendingMemberId),
                    $"Could not find member with id {request.SendingMemberId} in group with id {request.GroupId}"
                );
            }

            if (group.Members.All(m => m.Id != request.ReceivingMemberId))
            {
                _logger.LogDebug(
                    "No member found in group with id {Id}",
                    request.ReceivingMemberId
                );
                return Error.NotFound(
                    nameof(request.ReceivingMemberId),
                    $"Could not find member with id {request.ReceivingMemberId} in group with id {request.GroupId}"
                );
            }

            _logger.LogDebug("Verified existence of sending and receiving member in group");

            group.Payments.Add(
                new Payment
                {
                    SendingMemberId = request.SendingMemberId!.Value,
                    ReceivingMemberId = request.ReceivingMemberId!.Value,
                    Amount = request.Amount!.Value,
                    Timestamp = request.Timestamp!.Value,
                }
            );
            _logger.LogDebug("Mapped request to entity and added to Group's Payments collection");
            await _dbContext.SaveChangesAsync(CancellationToken.None);
            _logger.LogDebug("Persisted changes to database");

            return Result.Created;
        }
    }
}
