using Budgets.Contracts.GetBudgets;

namespace Budgets.UI.BudgetState;

internal sealed class BudgetStateService(IBudgetStateStorage budgetStateStorage): BudgetsStateProvider, IBudgetsStateService
{
    public override async Task<BudgetState> GetBudgetStateAsync()
    {
        return await budgetStateStorage.GetBudgetState() ?? new BudgetState();
    }

    public async ValueTask<bool> IsBudgetSelected(Budget budget)
    {
        if (await budgetStateStorage.GetBudgetState() is not { } selectedBudgetState) return false;
        return budget.Id == selectedBudgetState.BudgetId;
    }

    public ValueTask SelectBudget(Budget budget) =>
        budgetStateStorage.SaveBudgetState(new SelectedBudgetState(budget.Id, budget.Name));

    public ValueTask UnselectBudget() => budgetStateStorage.DeleteBudgetState();
}