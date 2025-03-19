using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SplitTheBill.Application.Common.Persistence;
using SplitTheBill.Application.Common.Validation;

namespace SplitTheBill.Application.Modules.Groups;

public static class UpdateGroup
{
    public sealed record Request : IRequest<ErrorOr<Updated>>
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
    }

    internal sealed class Validator : BaseValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Id)
                .NotEmptyWithErrorCode(ErrorCodes.Invalid);
            RuleFor(r => r.Name)
                .ValidString(true);
        }
    }

    internal sealed class Handler : IRequestHandler<Request, ErrorOr<Updated>>
    {
        #region construction

        private readonly ILogger<Handler> _logger;
        private readonly IAppDbContext _dbContext;

        public Handler(
            ILogger<Handler> logger,
            IAppDbContext dbContext
        )
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        #endregion

        public async Task<ErrorOr<Updated>> Handle(Request request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Updating Group with id {Id}", request.Id);

            var group = await _dbContext
                .Groups
                .SingleOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
            if (group is null)
            {
                _logger.LogDebug("No group with id {Id} found in database", request.Id);
                return Error.NotFound(nameof(request.Id), $"Could not find group with id {request.Id}");
            }
            _logger.LogDebug("Fetched entity to update from database");

            group.Name = request.Name!;
            _logger.LogDebug("Mapped updated properties from request to entity");
            
            await _dbContext.SaveChangesAsync(CancellationToken.None);
            _logger.LogDebug("Persisted updated entity to database");
            
            return Result.Updated;
        }
    }
}