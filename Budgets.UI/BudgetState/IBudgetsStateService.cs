using Budgets.Contracts.GetBudgets;

namespace Budgets.UI.BudgetState;

interface IBudgetsStateService
{
    ValueTask<bool> IsBudgetSelected(Budget budget);
    ValueTask SelectBudget(Budget budget);
    ValueTask UnselectBudget();
}