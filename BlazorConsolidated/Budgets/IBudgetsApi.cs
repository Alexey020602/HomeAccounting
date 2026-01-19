using BlazorConsolidated.Common.Attributes;
using ClientServerContracts.Budgets;
using ClientServerContracts.Budgets.CreateBudget;
using ClientServerContracts.Budgets.GetBudgetDetail;
using ClientServerContracts.Budgets.GetBudgets;
using ClientServerContracts.Budgets.UserInBudgetPermissions;
using Refit;

namespace BlazorConsolidated.Budgets;

[ApiAuthorizable("budgets")]
[Headers("Authorization: Bearer")]
public interface IBudgetsApi
{
    [Get("/")]
    public Task<IReadOnlyCollection<BudgetDto>> GetBudgets(GetBudgetsRequest request);

    [Post("/")] 
    public Task CreateBudget(CreateBudgetRequest request);

    [Get("/{id}")]
    public Task<BudgetDetailDto> GetBudgetDetail(Guid id);

    [Put("/{id}")]
    public Task UpdateBudget(Guid id, BudgetData budgetData);
    [Delete("/{id}")]
    public Task DeleteBudget(Guid id);
    [Get("/{id}/users")]
    public Task<IReadOnlyCollection<BudgetUserDto>> GetBudgetUsers(Guid id);
    [Get("/{id}/permissions")]
    public Task<UserInBudgetPermissionsDto> GetUserPermissions(Guid id);
}