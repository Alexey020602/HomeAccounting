using BlazorConsolidated.Common;

namespace BlazorConsolidated.Budgets.BudgetState;

internal sealed class BudgetStateStorage(ILocalStorage localStorage) : IBudgetStateStorage
{
    private const string BudgetsStateKey = "BudgetState";
    private static string GetBudgetStateKey(Guid userId) => $"{BudgetsStateKey}:{userId}";
    public ValueTask<SelectedBudgetState?> GetBudgetState(Guid userId, CancellationToken cancellationToken = default) =>
        localStorage.GetAsync<SelectedBudgetState>(GetBudgetStateKey(userId), cancellationToken: cancellationToken);

    public ValueTask SaveBudgetState(Guid userId, SelectedBudgetState budgetState,
        CancellationToken cancellationToken = default) =>
        localStorage.SetAsync(GetBudgetStateKey(userId), budgetState, cancellationToken:cancellationToken);

    public ValueTask DeleteBudgetState(Guid userId, CancellationToken cancellationToken = default) =>
        localStorage.RemoveAsync(GetBudgetStateKey(userId), cancellationToken);
}