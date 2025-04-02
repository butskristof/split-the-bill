using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SplitTheBill.Application.Common.Persistence;
using SplitTheBill.Application.Common.Validation;

namespace SplitTheBill.Application.Modules.Groups.Expenses;

public static class DeleteExpense
{
    public sealed record Request(Guid GroupId, Guid ExpenseId) : IRequest<ErrorOr<Deleted>>;

    internal sealed class Validator : BaseValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.GroupId)
                .NotEmptyWithErrorCode(ErrorCodes.Invalid);
            RuleFor(r => r.ExpenseId)
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
            _logger.LogDebug("Deleting Expense with id {ExpenseId} from group {GroupId}",
                request.ExpenseId, request.GroupId);

            var group = await _dbContext
                .CurrentUserGroups(true)
                .Include(g => g.Expenses)
                .SingleOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);
            if (group is null)
            {
                _logger.LogDebug("No group with id {GroupId} found in database", request.GroupId);
                return Error.NotFound(nameof(request.GroupId), $"Could not find group with id {request.GroupId}");
            }

            _logger.LogDebug("Fetched group to delete expense from from database");

            var expense = group.Expenses
                .SingleOrDefault(e => e.Id == request.ExpenseId);
            if (expense is null)
            {
                _logger.LogDebug("No expense with id {ExpenseId} found in group {GroupId}",
                    request.ExpenseId, request.GroupId);
                return Error.NotFound(nameof(request.ExpenseId), $"Could not find expense with id {request.ExpenseId}");
            }
            
            group.Expenses.Remove(expense);
            _logger.LogDebug("Deleted expense with id {ExpenseId} from group {GroupId}",
                request.ExpenseId, request.GroupId);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
            _logger.LogDebug("Persisted changes to database");
            
            return Result.Deleted;
        }
    }
}