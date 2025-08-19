using MaybeResults;

namespace Budgets.Core.CreateBudget;

public interface ICreateBudgetService
{
    // Task<bool> CheckBudgetExistAsync();
    Task<IMaybe> CreateAsync(NewBudget budget);
}