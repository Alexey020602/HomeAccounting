namespace Budgets.Core.GetBudgetUsers;

public interface IGetBudgetUsersService
{
    public Task<IReadOnlyCollection<Model.BudgetUser>> GetUsersForBudget(Guid budgetId);
}