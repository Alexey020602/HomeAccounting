using System.Net;
using System.Security.Claims;
using ClientServerShared.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MyBudgets.Budgets.Data;
using MyBudgets.Budgets.Data.Database;
using MyBudgets.Users.Data;

namespace MyBudgets.Budgets.DeleteBudget;

static class DeleteBudgetEndpoint
{
    public static void MapDeleteBudget(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
            "{id:guid}",
            async (Guid id, ClaimsPrincipal user, BudgetsContext budgetsContext, IAuthorizationService authorizationHandler, CancellationToken cancellationToken) =>
            {
                
                var budgetId = new BudgetId(id);
                var result = await authorizationHandler.AuthorizeAsync(user, budgetId, new BudgetRequirements(BudgetPermissions.Delete));
                if (!result.Succeeded)
                {
                    return Results.Problem(statusCode: (int)HttpStatusCode.Forbidden, detail: "User does not have permission to delete this budget");
                }

                var budget = await budgetsContext.Budgets
                    .Include(b => b.BudgetUsers)
                    .FirstOrDefaultAsync(b => b.Id == budgetId, cancellationToken);

                if (budget is null)
                {
                    return Results.NotFound();
                }

                budgetsContext.Budgets.Remove(budget);

                await budgetsContext.SaveChangesAsync(cancellationToken);

                return Results.NoContent();
            })
            .Produces((int)HttpStatusCode.NoContent)
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.Forbidden)
            .ProducesProblem((int)HttpStatusCode.BadRequest);
    }
}
