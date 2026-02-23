using ClientServerContracts.Budgets.GetBudgets;

namespace BlazorConsolidated.Budgets.BudgetState;

interface IBudgetsStateService
{
    ValueTask<bool> IsBudgetSelected(BudgetDto budget, CancellationToken cancellationToken = default);
    ValueTask SelectBudget(BudgetDto budget, CancellationToken cancellationToken = default);
    ValueTask UnselectBudget(CancellationToken cancellationToken = default);
}