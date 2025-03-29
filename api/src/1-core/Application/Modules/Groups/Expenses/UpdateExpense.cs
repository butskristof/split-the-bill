using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using SplitTheBill.Application.Common.Validation;

namespace SplitTheBill.Application.Modules.Groups.Expenses;

public static class UpdateExpense
{
    public sealed record Request : IRequest<ErrorOr<Updated>>
    {
        public Guid? GroupId { get; init; }
        public Guid? ExpenseId { get; init; }
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

        public Handler(ILogger<Handler> logger)
        {
            _logger = logger;
        }

        #endregion

        public Task<ErrorOr<Updated>> Handle(Request request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}