using Budgets.Core.EditBudget;
using Budgets.Core.GetBudgetDetail;
using Budgets.Core.Model;
using MaybeResults;

namespace Budgets.DataBase;

public sealed class UpdateBudgetService(BudgetsContext budgetsContext): IUpdateBudgetService
{
    public async Task<IMaybe> UpdateBudget(Budget budget, CancellationToken cancellationToken)
    {
        if (await budgetsContext.Budgets.FindAsync(cancellationToken: cancellationToken, keyValues:[budget.Id]) is not { } existingBudget)
        {
            return new BudgetNotFoundError($"Budget {budget.Id} not found");
        }
        
        budgetsContext.Budgets.Entry(existingBudget).CurrentValues.SetValues(budget);
        
        await budgetsContext.SaveChangesAsync(cancellationToken);
        return Maybe.Create();
    }
}