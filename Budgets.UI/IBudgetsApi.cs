using Budgets.Contracts.CreateBudget;
using Budgets.Contracts.GetBudgetDetail;
using Budgets.Contracts.GetBudgets;
using Budgets.Contracts.UserInBudgetPermissions;
using Refit;
using Shared.Blazor.Attributes;
using Shared.Utils;

namespace Budgets.UI;

[ApiAuthorizable("budgets")]
[Headers("Authorization: Bearer")]
public interface IBudgetsApi
{
    [Get("/")]
    public Task<IReadOnlyCollection<Budget>> GetBudgets(GetBudgetsHttpRequest request);

    [Post("/")] 
    public Task<ApiResponse<Unit>> CreateBudget(CreateBudgetHttpRequest request);

    [Get("/{id}")]
    public Task<ApiResponse<BudgetDetail>> GetBudgetDetail(Guid id);
    [Get("/{id}/users")]
    public Task<ApiResponse<IReadOnlyCollection<BudgetUser>>> GetBudgetUsers(Guid id);
    [Get("/{id}/permissions")]
    public Task<ApiResponse<UserInBudgetPermissions>> GetUserPermissions(Guid id);
}