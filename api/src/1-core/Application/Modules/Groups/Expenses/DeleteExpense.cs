using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using SplitTheBill.Application.Common.Validation;

namespace SplitTheBill.Application.Modules.Groups.Expenses;

public static class DeleteExpense
{
    public sealed record Request(Guid GroupId, Guid ExpenseId) : IRequest<ErrorOr<Deleted>>;

    internal sealed class Validator : BaseValidator<Request>
    {
        public Validator()
        {
        }
    }

    internal sealed class Handler : IRequestHandler<Request, ErrorOr<Deleted>>
    {
        #region construction

        private readonly ILogger<Handler> _logger;

        public Handler(ILogger<Handler> logger)
        {
            _logger = logger;
        }

        #endregion

        public Task<ErrorOr<Deleted>> Handle(Request request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}