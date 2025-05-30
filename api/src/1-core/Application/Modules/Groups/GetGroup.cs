using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SplitTheBill.Application.Common.Persistence;
using SplitTheBill.Application.Common.Validation;

namespace SplitTheBill.Application.Modules.Groups;

public static class GetGroup
{
    public sealed record Request(Guid Id) : IQuery<ErrorOr<GroupDto>>;

    internal sealed class Validator : BaseValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Id).NotEmptyWithErrorCode(ErrorCodes.Invalid);
        }
    }

    internal sealed class Handler : IQueryHandler<Request, ErrorOr<GroupDto>>
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

        public async ValueTask<ErrorOr<GroupDto>> Handle(
            Request request,
            CancellationToken cancellationToken
        )
        {
            _logger.LogDebug("Fetching Group with id {Id}", request.Id);

            var group = await _dbContext
                .CurrentUserGroups(false)
                .Include(g => g.Members)
                .Include(g => g.Expenses)
                .ThenInclude(e => e.Participants)
                .Include(g => g.Payments)
                .SingleOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
            if (group is null)
            {
                _logger.LogDebug("No group with id {Id} found in database", request.Id);
                return Error.NotFound(
                    nameof(request.Id),
                    $"Could not find group with id {request.Id}"
                );
            }
            _logger.LogDebug("Fetched (unmapped) entity from database");

            var groupDto = new GroupDto(group);
            return groupDto;
        }
    }
}
