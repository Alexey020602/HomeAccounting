using Microsoft.Extensions.Logging;

namespace BlazorConsolidated.Budgets.BudgetState;

internal sealed class TelemetryBudgetStateStorage(IBudgetStateStorage budgetStateStorage, ILogger<TelemetryBudgetStateStorage> logger) : IBudgetStateStorage
{
    public async ValueTask<SelectedBudgetState?> GetBudgetState(Guid userId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Getting budget state for user with id {UserId}", userId);
            var budgetState = await budgetStateStorage.GetBudgetState(userId, cancellationToken);
            logger.LogInformation("Budget state retrieved for user with id {UserId}", userId);
            return budgetState;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get budget state for user with id {UserId}", userId);
            throw;
        }
    }

    public async ValueTask SaveBudgetState(Guid userId, SelectedBudgetState budgetState,
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Saving budget state for user with id {UserId}", userId);
            await budgetStateStorage.SaveBudgetState(userId, budgetState, cancellationToken);
            logger.LogInformation("Budget state saved for user with id {UserId}", userId);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to save budget state for user with id {UserId}", userId);
            throw;
        }
    }

    public async ValueTask DeleteBudgetState(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Deleting budget state for user with id {UserId}", userId);
            await budgetStateStorage.DeleteBudgetState(userId, cancellationToken);
            logger.LogInformation("Budget state deleted for user with id {UserId}", userId);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to delete budget state for user with id {UserId}", userId);
            throw;
        }
    }
}