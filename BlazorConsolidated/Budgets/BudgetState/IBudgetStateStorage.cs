namespace BlazorConsolidated.Budgets.BudgetState;

internal interface IBudgetStateStorage
{
    public ValueTask<SelectedBudgetState?> GetBudgetState(Guid userId, CancellationToken cancellationToken = default);
    public ValueTask SaveBudgetState(Guid userId, SelectedBudgetState budgetState,
        CancellationToken cancellationToken = default);
    public ValueTask DeleteBudgetState(Guid userId, CancellationToken cancellationToken = default);
}