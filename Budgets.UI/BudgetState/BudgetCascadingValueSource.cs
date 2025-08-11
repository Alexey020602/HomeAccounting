using Microsoft.AspNetCore.Components;

namespace Budgets.UI.BudgetState;

public class BudgetCascadingValueSource : 
    CascadingValueSource<Task<BudgetState>>, IDisposable
{
    private readonly BudgetsStateProvider budgetsStateProvider;
    public BudgetCascadingValueSource(BudgetsStateProvider budgetsStateProvider) : base(budgetsStateProvider.GetBudgetStateAsync, isFixed: false)
    {
        this.budgetsStateProvider = budgetsStateProvider;
        budgetsStateProvider.BudgetStateChanged += HandleBudgetsStateChanged;
    }

    private void HandleBudgetsStateChanged(Task<UI.BudgetState.BudgetState> newBudgetsStateTask) =>
        NotifyChangedAsync(newBudgetsStateTask);

    void IDisposable.Dispose()
    {
        budgetsStateProvider.BudgetStateChanged -= HandleBudgetsStateChanged;
    }
}