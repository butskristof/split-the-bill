using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SplitTheBill.Application.Common.Persistence;

namespace SplitTheBill.Application.Modules.Members;

public static class GetMembers
{
    public sealed record Request : IQuery<ErrorOr<Response>>;

    public sealed record Response(List<Response.MemberDto> Members)
    {
        public sealed record MemberDto(Guid Id, string Name);
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
            _logger.LogDebug("Fetching all members from database");

            var members = await _dbContext
                .Members.AsNoTracking()
                .Select(m => new Response.MemberDto(m.Id, m.Name))
                .ToListAsync(cancellationToken);
            _logger.LogDebug("Fetched mapped Member entities from database");

            return new Response(members);
        }
    }
}
