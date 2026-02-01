using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets.GetBudgetDetail;
using ClientServerShared.Model;
using HomeAccounting.Budgets.Data;
using HomeAccounting.Budgets.Data.Database;
using HomeAccounting.Users.GetUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HomeAccounting.Budgets;
using HomeAccounting.Users.Data;
using HomeAccounting.Users.Data.Database;

namespace HomeAccounting.Budgets.GetBudgetUsers;

/// <summary>
/// Endpoint for retrieving users of a budget.
/// </summary>
static class GetBudgetUsersEndpoint
{
    /// <summary>
    /// Maps GET /budgets/{id}/users. Returns list of users in the budget; requires read permission.
    /// </summary>
    public static void MapGetBudgetUsers(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "{id:guid}/users",
            async (Guid id, ClaimsPrincipal user, BudgetsContext budgetsContext, IUsersService usersService, IAuthorizationService authorizationHandler, CancellationToken cancellationToken) =>
            {
                var budgetId = new BudgetId(id);
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
                
                var userIds = budget.BudgetUsers.Select(b => b.UserId.Value);
                var rolesIds = budget.BudgetUsers.Select(u => u.BudgetRoleId);

                var users = await usersService.GetUsers(userIds, cancellationToken);
                
                var roles = await budgetsContext.BudgetRoles
                    .Where(r => rolesIds.Contains(r.Id))
                    .ToListAsync(cancellationToken);
                
                var budgetUsers = from user1 in users
                    join budgetUser in budget.BudgetUsers on user1.Id equals budgetUser.UserId.Value
                    join role in roles on budgetUser.BudgetRoleId equals role.Id 
                    select new BudgetUserDto(user1.Id, user1.UserName!, role.Name);
                
                return Results.Ok(budgetUsers.ToList());
            })
            .WithName("GetBudgetUsers")
            .WithTags("Budgets")
            .WithSummary("Get budget users")
            .WithDescription("Returns list of users in the budget with their roles. Requires read permission.")
            .Produces((int)HttpStatusCode.OK, typeof(IReadOnlyCollection<BudgetUserDto>))
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.Forbidden)
            .ProducesProblem((int)HttpStatusCode.BadRequest);
    }
}
