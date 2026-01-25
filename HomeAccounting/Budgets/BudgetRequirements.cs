using HomeAccounting.Budgets.Data;
using Microsoft.AspNetCore.Authorization;

namespace HomeAccounting.Budgets;

internal sealed record BudgetRequirements(BudgetPermissions Permission) : IAuthorizationRequirement;