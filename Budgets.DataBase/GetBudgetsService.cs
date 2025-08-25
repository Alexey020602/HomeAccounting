using Budgets.Contracts.GetBudgets;
using Budgets.Core.GetBudgets;
using MaybeResults;
using Microsoft.EntityFrameworkCore;
using Shared.Utils.Results;

namespace Budgets.DataBase;

public sealed class GetBudgetsService(BudgetsContext dbContext): IGetBudgetsService
{
    public async Task<IMaybe<IReadOnlyCollection<Budget>>> GetBudgets(BudgetsRequest request)
    {
        return await dbContext.BudgetUsers
            .AsNoTracking()
            .Where(request.FilterExpression())
            .Include(user => user.Budget)
            .Select(budgetUser => ConvertToBudget(budgetUser.Budget))
            .ToListAsync()
            .TryAsync<IReadOnlyCollection<Budget>, List<Budget>>();
    }
    
    private static Budget ConvertToBudget(Core.Model.Budget? budget)
    {
        ArgumentNullException.ThrowIfNull(budget);
        if (budget.Name is null)
        {
            throw new BudgetsDataBaseException("Name attribute of budget must not be null");
        }
        
        return new Budget(
            budget.Id,
            budget.Name
        );
    }
}