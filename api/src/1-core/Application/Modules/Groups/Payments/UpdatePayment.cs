using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using SplitTheBill.Application.Common.Persistence;
using SplitTheBill.Application.Common.Validation;

namespace SplitTheBill.Application.Modules.Groups.Payments;

public static class UpdatePayment
{
    public sealed record Request : IRequest<ErrorOr<Updated>>
    {
        public Guid? GroupId { get; init; }
        public Guid? PaymentId { get; init; }
        public Guid? SendingMemberId { get; init; }
        public Guid? ReceivingMemberId { get; init; }
        public decimal? Amount { get; init; }
    }

    internal sealed class Validator : BaseValidator<Request>
    {
        public Validator()
        {
        }
    }

    internal sealed class Handler : IRequestHandler<Request, ErrorOr<Updated>>
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

        public async Task<ErrorOr<Updated>> Handle(Request request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}