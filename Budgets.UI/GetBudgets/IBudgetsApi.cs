using Budgets.Contracts.CreateBudget;
using Budgets.Contracts.GetBudgetDetail;
using Budgets.Contracts.GetBudgets;
using Refit;
using Shared.Blazor.Attributes;
using Shared.Utils;

namespace Budgets.UI.GetBudgets;

[ApiAuthorizable("budgets")]
[Headers("Authorization: Bearer")]
public interface IBudgetsApi
{
    [Get("/")]
    public Task<IReadOnlyCollection<Budget>> GetBudgets(GetBudgetsHttpRequest request);

    [Post("/")] 
    public Task<ApiResponse<Unit>> CreateBudget(CreateBudgetHttpRequest request);

    [Get("/{id}")]
    public Task<ApiResponse<BudgetFullDetail>> GetBudgetDetail(Guid id);
}