using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets.GetBudgetDetail;
using ClientServerShared.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBudgets.Budgets;
using MyBudgets.Budgets.Data;
using MyBudgets.Budgets.Data.Database;
using MyBudgets.Users.Data;
using MyBudgets.Users.Data.Database;

namespace MyBudgets.Budgets.GetBudgetUsers;

static class GetBudgetUsersEndpoint
{
    public static void MapGetBudgetUsers(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "{id:guid}/users",
            async (Guid id, ClaimsPrincipal user, BudgetsContext budgetsContext, UserManager<User> userManager, IAuthorizationService authorizationHandler, CancellationToken cancellationToken) =>
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
                
                var userIds = budget.BudgetUsers.Select(b => b.UserId);
                var rolesIds = budget.BudgetUsers.Select(u => u.BudgetRoleId);

                var users = await userManager.Users
                    .Where(u => userIds.Contains(u.Id))
                    // .Where(u => rolesIds.Contains(u.Id))
                    .ToListAsync(cancellationToken);
                
                var roles = await budgetsContext.BudgetRoles
                    .Where(r => rolesIds.Contains(r.Id))
                    .ToListAsync(cancellationToken);
                
                var budgetUsers = from user1 in users
                    join budgetUser in budget.BudgetUsers on user1.Id equals budgetUser.UserId
                    join role in roles on budgetUser.BudgetRoleId equals role.Id 
                    select new BudgetUserDto(user1.Id.Value, user1.UserName!, role.Name);
                
                return Results.Ok(budgetUsers.ToList());
            })
            .Produces((int)HttpStatusCode.OK, typeof(IReadOnlyCollection<BudgetUserDto>))
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.Forbidden)
            .ProducesProblem((int)HttpStatusCode.BadRequest);
    }
}
