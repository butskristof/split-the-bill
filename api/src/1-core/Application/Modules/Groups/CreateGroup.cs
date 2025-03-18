using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SplitTheBill.Application.Common.Persistence;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.Modules.Groups;

public static class CreateGroup
{
    public sealed record Request : IRequest<ErrorOr<Response>>
    {
        public string? Name { get; init; }
    }

    public sealed record Response(Guid Id);

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(r => r.Name)
                .NotNullWithErrorCode(ErrorCodes.Required)
                .NotEmptyWithErrorCode(ErrorCodes.Required);
        }
    }

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
            _logger.LogDebug("Creating new Group");

            var group = new Group
            {
                Name = request.Name!,
            };
            _logger.LogDebug("Mapped request to entity");

            _dbContext.Groups.Add(group);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
            _logger.LogDebug("Persisted new entity to database with id {Id}", group.Id);

            return new Response(group.Id);
        }
    }
}