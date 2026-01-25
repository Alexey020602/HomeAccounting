using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets.UserInBudgetPermissions;
using ClientServerShared.Model;
using HomeAccounting.Budgets.Data;
using HomeAccounting.Budgets.Data.Database;
using HomeAccounting.Users.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HomeAccounting.Budgets;

namespace HomeAccounting.Budgets.GetUserPermissions;

static class GetUserPermissionsEndpoint
{
    public static void MapGetUserPermissions(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "{id:guid}/permissions",
            async (Guid id, ClaimsPrincipal user, BudgetsContext budgetsContext, IAuthorizationService authorizationHandler, CancellationToken cancellationToken) =>
            {
                var budgetId = new BudgetId(id);
                var userId = new UserId(user.GetUserId());
                
                var result = await authorizationHandler.AuthorizeAsync(user, budgetId, new BudgetRequirements(BudgetPermissions.Read));
                if (!result.Succeeded)
                {
                    return Results.Problem(statusCode: (int)HttpStatusCode.Forbidden, detail: "User does not have permission to read this budget");
                }

                var budget = await budgetsContext.Budgets
                    .Include(b => b.BudgetUsers)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.Id == budgetId, cancellationToken);

                if (budget is null)
                {
                    return Results.NotFound();
                }

                var budgetRoleId = budget.GetUserRole(userId);
                if (budgetRoleId is null)
                {
                    return Results.Problem(statusCode: (int)HttpStatusCode.Forbidden, detail: "User does not have access to this budget");
                }

                var userRole = await budgetsContext.BudgetRoles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == budgetRoleId, cancellationToken);

                if (userRole is null)
                {
                    return Results.Problem(statusCode: (int)HttpStatusCode.Forbidden, detail: "User role not found");
                }

                var response = new UserInBudgetPermissionsDto(budgetId.Value, userRole.CanUserEdit, userRole.CanUserDelete);

                return Results.Ok(response);
            })
            .Produces((int)HttpStatusCode.OK, typeof(UserInBudgetPermissionsDto))
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.Forbidden)
            .ProducesProblem((int)HttpStatusCode.BadRequest);
    }
}
