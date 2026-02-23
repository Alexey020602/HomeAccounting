namespace BlazorConsolidated.Budgets.BudgetState;

internal interface IBudgetStateStorage
{
    public ValueTask<SelectedBudgetState?> GetBudgetState(CancellationToken cancellationToken = default);
    public ValueTask SaveBudgetState(SelectedBudgetState budgetState, CancellationToken cancellationToken = default);
    public ValueTask DeleteBudgetState(CancellationToken cancellationToken = default);
}