using BlazorConsolidated.Common.Attributes;
using Budgets.Contracts;
using Budgets.Contracts.CreateBudget;
using Budgets.Contracts.GetBudgetDetail;
using Budgets.Contracts.UserInBudgetPermissions;
using ClientServerContracts.Budgets.GetBudgets;
using Refit;

namespace BlazorConsolidated.Budgets;

[ApiAuthorizable("budgets")]
[Headers("Authorization: Bearer")]
public interface IBudgetsApi
{
    [Get("/")]
    public Task<IReadOnlyCollection<Budget>> GetBudgets(GetBudgetsHttpRequest request);

    [Post("/")] 
    public Task CreateBudget(CreateBudgetHttpRequest request);

    [Get("/{id}")]
    public Task<BudgetDetail> GetBudgetDetail(Guid id);

    [Put("/{id}")]
    public Task UpdateBudget(Guid id, BudgetData budgetData);
    [Delete("/{id}")]
    public Task DeleteBudget(Guid id);
    [Get("/{id}/users")]
    public Task<IReadOnlyCollection<BudgetUser>> GetBudgetUsers(Guid id);
    [Get("/{id}/permissions")]
    public Task<UserInBudgetPermissions> GetUserPermissions(Guid id);
}