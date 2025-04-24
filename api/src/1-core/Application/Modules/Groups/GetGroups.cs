using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SplitTheBill.Application.Common.Persistence;

namespace SplitTheBill.Application.Modules.Groups;

public static class GetGroups
{
    public sealed record Request : IQuery<ErrorOr<Response>>;

    public sealed record Response(List<Response.GroupDto> Groups)
    {
        public sealed record GroupDto(Guid Id, string Name);
    }

    internal sealed class Handler : IQueryHandler<Request, ErrorOr<Response>>
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

        public async ValueTask<ErrorOr<Response>> Handle(
            Request request,
            CancellationToken cancellationToken
        )
        {
            _logger.LogDebug("Fetching all groups from database");

            var groups = await _dbContext
                .CurrentUserGroups(false)
                .Select(g => new Response.GroupDto(g.Id, g.Name))
                .ToListAsync(cancellationToken);
            _logger.LogDebug("Fetched mapped Group entities from database");

            return new Response(groups);
        }
    }
}
