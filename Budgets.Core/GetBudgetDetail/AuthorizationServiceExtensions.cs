using System.Security.Claims;
using Budgets.Core.UserInBudgetPermissions;
using MaybeResults;
using Microsoft.AspNetCore.Authorization;

namespace Budgets.Core.GetBudgetDetail;

public static class AuthorizationServiceExtensions
{
    public static async Task<bool> CheckUserHasPermission(this IAuthorizationService authorizationService,
        ClaimsPrincipal user, Guid budgetId,
        BudgetPermissions permissions) =>
        (await authorizationService.AuthorizeAsync(user, budgetId, new BudgetRequirements(permissions))).Succeeded;
}