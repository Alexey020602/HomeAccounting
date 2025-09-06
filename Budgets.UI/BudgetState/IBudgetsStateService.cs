using Budgets.Contracts.GetBudgets;

namespace Budgets.UI.BudgetState;

interface IBudgetsStateService
{
    ValueTask<bool> IsBudgetSelected(Budget budget, CancellationToken cancellationToken = default);
    ValueTask SelectBudget(Budget budget, CancellationToken cancellationToken = default);
    ValueTask UnselectBudget(CancellationToken cancellationToken = default);
}