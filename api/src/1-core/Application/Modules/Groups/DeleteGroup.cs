using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SplitTheBill.Application.Common.Persistence;
using SplitTheBill.Application.Common.Validation;

namespace SplitTheBill.Application.Modules.Groups;

public static class DeleteGroup
{
    public sealed record Request(Guid Id) : ICommand<ErrorOr<Deleted>>;

    internal sealed class Validator : BaseValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Id)
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
            _logger.LogDebug("Deleting Group with id {Id}", request.Id);

            var group = await _dbContext
                .CurrentUserGroups(true)
                .SingleOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
            if (group is null)
            {
                _logger.LogDebug("No group with id {Id} found in database", request.Id);
                return Error.NotFound(nameof(request.Id), $"Could not find group with id {request.Id}");
            }

            _logger.LogDebug("Fetched entity to delete from database");

            _dbContext.Groups.Remove(group);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
            _logger.LogDebug("Deleted group with id {Id}", request.Id);

            return Result.Deleted;
        }
    }
}