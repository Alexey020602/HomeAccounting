using ClientServerContracts.Budgets.GetBudgets;

namespace BlazorConsolidated.Budgets.BudgetState;

interface IBudgetsStateService
{
    ValueTask<bool> IsBudgetSelected(Budget budget, CancellationToken cancellationToken = default);
    ValueTask SelectBudget(Budget budget, CancellationToken cancellationToken = default);
    ValueTask UnselectBudget(CancellationToken cancellationToken = default);
}