using System.Text.Json.Serialization;

namespace ClientServerContracts.Budgets.UserInBudgetPermissions;

public sealed record UserInBudgetPermissionsDto(Guid BudgetId, bool CanEdit, bool CanDelete);