using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SplitTheBill.Application.Common.Persistence;
using SplitTheBill.Application.Common.Validation;

namespace SplitTheBill.Application.Modules.Members;

public static class DeleteMember
{
    public sealed record Request(Guid Id) : IRequest<ErrorOr<Deleted>>;

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Id)
                .NotEmptyWithErrorCode(ErrorCodes.Invalid);
        }
    }

    internal sealed class Handler : IRequestHandler<Request, ErrorOr<Deleted>>
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

        public async Task<ErrorOr<Deleted>> Handle(Request request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Deleting Member with id {Id}", request.Id);

            var member = await _dbContext
                .Members
                .SingleOrDefaultAsync(m => m.Id == request.Id, cancellationToken);
            if (member is null)
            {
                _logger.LogDebug("No member with id {Id} found in database", request.Id);
                return Error.NotFound(nameof(request.Id), $"Could not find member with id {request.Id}");
            }

            _logger.LogDebug("Fetched entity to delete from database");

            _dbContext.Members.Remove(member);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
            _logger.LogDebug("Deleted member with id {Id}", request.Id);

            return Result.Deleted;
        }
    }
}