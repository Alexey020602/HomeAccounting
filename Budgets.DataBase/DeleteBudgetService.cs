using Authorization.Contracts;
using Budgets.Core.DeleteBudget;
using MaybeResults;
using Microsoft.EntityFrameworkCore;

namespace Budgets.DataBase;

public sealed class DeleteBudgetService(BudgetsContext budgetsContext): IDeleteBudgetService
{
    public async Task<IMaybe> DeleteBudget(Guid budgetId)
    {
        if (await budgetsContext.Budgets.SingleOrDefaultAsync(b => b.Id == budgetId) is not {} existedBudget)
        {
            return new UserNotFoundError("Budget not found");
        }
        
        budgetsContext.Budgets.Remove(existedBudget);
        await budgetsContext.SaveChangesAsync();
        return Maybe.Create();
    }
}