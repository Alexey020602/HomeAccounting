using Budgets.Core.GetBudgetDetail;
using Budgets.Core.Model;
using MaybeResults;
using Microsoft.EntityFrameworkCore;

namespace Budgets.DataBase;

public sealed class GetBudgetDetailService(BudgetsContext budgetsContext): IGetBudgetDetailService
{
    public async Task<IMaybe<Budget>> GetBudgetFullDetail(Guid id, CancellationToken cancellationToken = default)
    {
        if (await budgetsContext
                .Budgets
                .AsNoTracking()
                .Include(budget => budget.BudgetUsers)
                .ThenInclude(user => user.BudgetRole)
                .FirstOrDefaultAsync(
                budget => budget.Id == id,
                cancellationToken: cancellationToken
            ) is { } budget)
        {
            return Maybe.Create(budget);
        }

        return new BudgetNotFoundError<Budget>(id);
    }
}