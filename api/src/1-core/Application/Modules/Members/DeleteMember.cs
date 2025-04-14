using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SplitTheBill.Application.Common.Persistence;
using SplitTheBill.Application.Common.Validation;

namespace SplitTheBill.Application.Modules.Members;

public static class DeleteMember
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
            _logger.LogDebug("Deleting Member with id {Id}", request.Id);

            var member = await _dbContext
                .Members
                .Include(m => m.Groups)
                .SingleOrDefaultAsync(m => m.Id == request.Id, cancellationToken);
            if (member is null)
            {
                _logger.LogDebug("No member with id {Id} found in database", request.Id);
                return Error.NotFound(nameof(request.Id), $"Could not find member with id {request.Id}");
            }

            _logger.LogDebug("Fetched entity to delete from database");

            if (member.Groups.Count > 0)
            {
                _logger.LogDebug("Member is part of one or more groups and cannot be deleted");
                return Error.Validation(nameof(request.Id), $"MemberIsInGroups");
            }

            _dbContext.Members.Remove(member);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
            _logger.LogDebug("Deleted member with id {Id}", request.Id);

            return Result.Deleted;
        }
    }
}