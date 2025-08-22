using Shared.Utils.MediatorWithResults;

namespace Budgets.Core.UserInBudgetPermissions;

public record UserInBudgetPermissionsQuery(Guid UserId, Guid BudgetId): IResultQuery<Contracts.UserInBudgetPermissions.UserInBudgetPermissions>;