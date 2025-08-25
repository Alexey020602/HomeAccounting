using Microsoft.Extensions.Logging;

namespace Budgets.UI.BudgetState;

internal sealed class TelemetryBudgetStateStorage(IBudgetStateStorage budgetStateStorage, ILogger<TelemetryBudgetStateStorage> logger) : IBudgetStateStorage
{
    public async ValueTask<SelectedBudgetState?> GetBudgetState(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Getting budget state");
            var budgetState = await budgetStateStorage.GetBudgetState(cancellationToken);
            logger.LogInformation("Budget state retrieved");
            return budgetState;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get budget state");
            throw;
        }
    }

    public async ValueTask SaveBudgetState(SelectedBudgetState budgetState, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Saving budget state");
            await budgetStateStorage.SaveBudgetState(budgetState, cancellationToken);
            logger.LogInformation("Budget state saved");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to save budget state");
            throw;
        }
    }

    public async ValueTask DeleteBudgetState(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Deleting budget state");
            await budgetStateStorage.DeleteBudgetState(cancellationToken);
            logger.LogInformation("Budget state deleted");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to delete budget state");
            throw;
        }
    }
}