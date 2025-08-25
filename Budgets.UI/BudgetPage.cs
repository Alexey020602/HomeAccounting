using Budgets.UI.BudgetState;
using Microsoft.AspNetCore.Components;

namespace Budgets.UI;

public abstract partial class BudgetPage: ComponentBase
{
    [CascadingParameter] protected Task<BudgetState.BudgetState>? BudgetState { get; set; }
    protected Guid BudgetId { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    protected override async Task OnParametersSetAsync()
    {
        if (BudgetState is null)
        {
            throw new InvalidOperationException("BudgetState is not set");
        }

        if (await BudgetState is not SelectedBudgetState selectedBudgetState)
        {
            NavigationManager.NavigateTo("/budgets");
            return;
        }
        
        BudgetId = selectedBudgetState.BudgetId;
    }
}