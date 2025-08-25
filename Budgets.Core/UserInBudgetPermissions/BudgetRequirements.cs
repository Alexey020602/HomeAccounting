using Microsoft.AspNetCore.Authorization;

namespace Budgets.Core.UserInBudgetPermissions;

public sealed record BudgetRequirements(BudgetPermissions Permission) : IAuthorizationRequirement;