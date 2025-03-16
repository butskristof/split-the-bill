using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SplitTheBill.Application.Common.Persistence;

namespace SplitTheBill.Application.Modules.Groups;

public static class GetGroups
{
    public sealed record Request : IRequest<ErrorOr<Response>>;

    public sealed record Response(
        List<Application.Common.Dto.GroupDto> Groups
    );

    internal sealed class Handler : IRequestHandler<Request, ErrorOr<Response>>
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

        public async Task<ErrorOr<Response>> Handle(Request request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Fetching all groups from database");

            var groups = await _dbContext
                .Groups
                .AsNoTracking()
                .Select(g => new Application.Common.Dto.GroupDto(g.Id, g.Name))
                .ToListAsync(cancellationToken);
            _logger.LogDebug("Fetched mapped Group entities from database");

            return new Response(groups);
        }
    }
}