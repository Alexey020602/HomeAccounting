using BlazorConsolidated.Users.Dto;
using BlazorConsolidated.Users.Infrastructure.Abstractions;
using ClientServerContracts.Budgets.GetBudgets;

namespace BlazorConsolidated.Budgets.BudgetState;

internal sealed class BudgetStateService(IBudgetStateStorage budgetStateStorage, IAuthenticationStorage authenticationStorage): BudgetsStateProvider, IBudgetsStateService
{
    public override async Task<BudgetState> GetBudgetStateAsync()
    {
        var authentication = await authenticationStorage.GetAuthorizationAsync();
        if (!TryGetCurrentUserIdAsync(authentication, out var userId)) 
            return new BudgetState();
        
        return await budgetStateStorage.GetBudgetState(userId) ?? new BudgetState();
    }

    public async ValueTask<bool> IsBudgetSelected(BudgetDto budget, CancellationToken cancellationToken = default)
    {
        var authentication = await authenticationStorage.GetAuthorizationAsync(cancellationToken);
        if (!TryGetCurrentUserIdAsync(authentication, out var userId)) 
            return false;
        if (await budgetStateStorage.GetBudgetState(userId, cancellationToken) is not { } selectedBudgetState) return false;
        return budget.Id == selectedBudgetState.BudgetId;
    }

    public async ValueTask SelectBudget(BudgetDto budget, CancellationToken cancellationToken = default)
    {
        var authentication = await authenticationStorage.GetAuthorizationAsync(cancellationToken);
        if (!TryGetCurrentUserIdAsync(authentication, out var userId)) 
            throw new InvalidOperationException("Необходимо авторизоваться для выбора бюджета");//todo Подумать над тем, чтобы добавить исключение при попытке выбрать бюджет без выбранного пользователя
        await budgetStateStorage.SaveBudgetState(userId, new SelectedBudgetState(budget.Id, budget.Name), cancellationToken);
        await NotifyBudgetStateChanged(Task.FromResult<BudgetState>(new SelectedBudgetState(budget.Id, budget.Name)));
    }

    public async ValueTask UnselectBudget(CancellationToken cancellationToken = default)
    {
        var authentication = await authenticationStorage.GetAuthorizationAsync(cancellationToken);
        if (!TryGetCurrentUserIdAsync(authentication, out var userId))
            throw new InvalidOperationException("Необходимо авторизоваться для снятия выбора с бюджета");//todo Подумать над тем, чтобы добавить исключение при попытке удалить бюджет без выбранного пользователя
        await budgetStateStorage.DeleteBudgetState(userId, cancellationToken);
        await NotifyBudgetStateChanged(Task.FromResult(new BudgetState()));
    }

    private static bool TryGetCurrentUserIdAsync(Authentication? authentication, out Guid userIdOut)
    {
        if (authentication is not { User:
            {
                Id: var userId
            } })
        {
            userIdOut = Guid.Empty;
            return false;
        }

        userIdOut = userId;
        return true;
    }
}