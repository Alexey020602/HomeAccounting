using System.Text.Json.Serialization;

namespace Budgets.Contracts.UserInBudgetPermissions;

[method: JsonConstructor]
public record UserInBudgetPermissions(Guid BudgetId, bool CanEdit, bool CanDelete)
{
    
}