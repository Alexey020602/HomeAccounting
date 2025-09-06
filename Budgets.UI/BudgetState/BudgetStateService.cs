using Budgets.Contracts.GetBudgets;
using Shared.Blazor.Logout;

namespace Budgets.UI.BudgetState;

internal sealed class BudgetStateService(IBudgetStateStorage budgetStateStorage): BudgetsStateProvider, IBudgetsStateService
{
    public override async Task<BudgetState> GetBudgetStateAsync()
    {
        return await budgetStateStorage.GetBudgetState() ?? new BudgetState();
    }

    public async ValueTask<bool> IsBudgetSelected(Budget budget, CancellationToken cancellationToken = default)
    {
        if (await budgetStateStorage.GetBudgetState(cancellationToken) is not { } selectedBudgetState) return false;
        return budget.Id == selectedBudgetState.BudgetId;
    }

    public async ValueTask SelectBudget(Budget budget, CancellationToken cancellationToken = default)
    {
        await budgetStateStorage.SaveBudgetState(new SelectedBudgetState(budget.Id, budget.Name), cancellationToken);
        await NotifyBudgetStateChanged(Task.FromResult<BudgetState>(new SelectedBudgetState(budget.Id, budget.Name)));
    }

    public async ValueTask UnselectBudget(CancellationToken cancellationToken = default)
    {
        await budgetStateStorage.DeleteBudgetState(cancellationToken);
        await NotifyBudgetStateChanged(Task.FromResult(new BudgetState()));
    }
}

internal sealed class BudgetsLogoutAction(IBudgetsStateService budgetsStateService): ILogoutAction
{
    public Task Logout(CancellationToken cancellationToken = default)
    {
        return budgetsStateService.UnselectBudget(cancellationToken).AsTask();
    }
}