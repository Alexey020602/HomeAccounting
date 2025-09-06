using Microsoft.AspNetCore.Components;
using Shared.Blazor.Components;

namespace Budgets.UI;

public class BudgetRequiredView: BudgetsSelectionView
{
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        BudgetNotSelected = _ => RedirectToBudgetsFragment;
    }

    private static RenderFragment RedirectToBudgetsFragment =>
        builder =>
        {
            builder.OpenComponent<RedirectComponent>(0);
            builder.AddComponentParameter(1, nameof(RedirectComponent.RedirectUrl), "budgets");
            builder.CloseComponent();
        }; //=> @<RedirectComponent RedirectUrl="budgets" />
}