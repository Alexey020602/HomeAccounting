using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets.AddUsersInBudget;
using ClientServerShared.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBudgets.Budgets;
using MyBudgets.Budgets.Data;
using MyBudgets.Budgets.Data.Database;
using MyBudgets.Users.Data;

namespace MyBudgets.Budgets.AddUsersInBudget;

static class AddUsersInBudgetEndpoint
{
    public static void MapAddUsersInBudget(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "{id:guid}/users",
            async (Guid id, ClaimsPrincipal user, AddUsersInBudgetRequest request, BudgetsContext budgetsContext, UserManager<User> userManager, IAuthorizationService authorizationHandler, CancellationToken cancellationToken) =>
            {
                var budgetId = new BudgetId(id);
                var result = await authorizationHandler.AuthorizeAsync(user, budgetId, new BudgetRequirements(BudgetPermissions.Edit));
                if (!result.Succeeded)
                {
                    return Results.Problem(statusCode: (int)HttpStatusCode.Forbidden, detail: "User does not have permission to edit this budget");
                }

                var budget = await budgetsContext.Budgets
                    .Include(b => b.BudgetUsers)
                    .FirstOrDefaultAsync(b => b.Id == budgetId, cancellationToken);

                if (budget is null)
                {
                    return Results.NotFound();
                }

                var userIdToAdd = new UserId(request.UserId);
                
                // Проверяем, что пользователь существует
                var userToAdd = await userManager.FindByIdAsync(userIdToAdd);
                if (userToAdd is null)
                {
                    return Results.Problem(statusCode: (int)HttpStatusCode.BadRequest, detail: "User not found");
                }

                // Проверяем, что пользователь еще не добавлен в бюджет
                if (budget.BudgetUsers.Any(bu => bu.UserId == userIdToAdd))
                {
                    return Results.Problem(statusCode: (int)HttpStatusCode.BadRequest, detail: "User is already added to this budget");
                }

                // Определяем роль: если не указана, используем роль "Пользователь" по умолчанию

                var roleId = new BudgetRoleId(request.RoleId);
                var roleExists = await budgetsContext.BudgetRoles
                        .AnyAsync(r => r.Id == roleId, cancellationToken);
                    
                if (!roleExists)
                {
                    return Results.Problem(statusCode: (int)HttpStatusCode.BadRequest, detail: "Role not found");
                }
                    

                try
                {
                    budget.AddUser(userIdToAdd, roleId);
                    await budgetsContext.SaveChangesAsync(cancellationToken);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Problem(statusCode: (int)HttpStatusCode.BadRequest, detail: ex.Message);
                }

                return Results.Created();
            })
            .Produces((int)HttpStatusCode.Created)
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.Forbidden)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.InternalServerError);
    }
}
