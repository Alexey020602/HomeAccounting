using Microsoft.AspNetCore.Authorization;
using MyBudgets.Budgets.Data;

namespace MyBudgets.Budgets;

internal sealed record BudgetRequirements(BudgetPermissions Permission) : IAuthorizationRequirement;