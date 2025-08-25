using Shared.Blazor;

namespace Budgets.UI.BudgetState;

internal sealed class BudgetStateStorage(ILocalStorage localStorage) : IBudgetStateStorage
{
    private const string BudgetsStateKey = "BudgetState";

    public ValueTask<SelectedBudgetState?> GetBudgetState(CancellationToken cancellationToken = default) =>
        localStorage.GetAsync<SelectedBudgetState>(BudgetsStateKey, cancellationToken: cancellationToken);

    public ValueTask SaveBudgetState(SelectedBudgetState budgetState, CancellationToken cancellationToken = default) =>
        localStorage.SetAsync(BudgetsStateKey, budgetState, cancellationToken);

    public ValueTask DeleteBudgetState(CancellationToken cancellationToken = default) =>
        localStorage.RemoveAsync(BudgetsStateKey, cancellationToken);
}