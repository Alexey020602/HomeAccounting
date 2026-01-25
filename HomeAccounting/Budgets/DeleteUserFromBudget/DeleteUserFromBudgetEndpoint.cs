using System.Net;
using System.Security.Claims;
using ClientServerShared.Model;
using HomeAccounting.Budgets.Data;
using HomeAccounting.Budgets.Data.Database;
using HomeAccounting.Users.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HomeAccounting.Budgets;

namespace HomeAccounting.Budgets.DeleteUserFromBudget;

static class DeleteUserFromBudgetEndpoint
{
    public static void MapDeleteUserFromBudget(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
            "{id:guid}/users/{userId:guid}",
            async (Guid id, Guid userId, ClaimsPrincipal user, BudgetsContext budgetsContext, IAuthorizationService authorizationHandler, CancellationToken cancellationToken) =>
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

                var currentUserId = new UserId(user.GetUserId());
                var userIdToRemove = new UserId(userId);

                try
                {
                    budget.RemoveUser(userIdToRemove, currentUserId);
                    await budgetsContext.SaveChangesAsync(cancellationToken);
                }
                catch (DomainException ex)
                {
                    return Results.Problem(statusCode: (int)HttpStatusCode.BadRequest, detail: ex.Message);
                }

                return Results.NoContent();
            })
            .Produces((int)HttpStatusCode.NoContent)
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.Forbidden)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.InternalServerError);
    }
}
