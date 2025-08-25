using Budgets.Core.GetBudgetUsers;
using Budgets.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace Budgets.DataBase;

public sealed class GetBudgetUsersService(BudgetsContext budgetsContext): IGetBudgetUsersService
{
    public async Task<IReadOnlyCollection<BudgetUser>> GetUsersForBudget(Guid budgetId) => await budgetsContext
        .BudgetUsers
        .Include(user => user.BudgetRole)
        .Where(user => user.BudgetId == budgetId)
        .ToListAsync();
}