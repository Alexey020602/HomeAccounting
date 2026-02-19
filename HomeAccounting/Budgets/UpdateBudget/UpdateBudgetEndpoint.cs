using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets;
using ClientServerShared.Model;
using HomeAccounting.Budgets.Data;
using HomeAccounting.Budgets.Data.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HomeAccounting.Budgets;
using HomeAccounting.Common.Model.ValueObjects;

namespace HomeAccounting.Budgets.UpdateBudget;

/// <summary>
/// Endpoint for updating an existing budget.
/// </summary>
static class UpdateBudgetEndpoint
{
    /// <summary>
    /// Maps PUT /budgets/{id}. Updates budget; requires edit permission.
    /// </summary>
    public static void MapUpdateBudget(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
            "{id:guid}",
            async (Guid id, ClaimsPrincipal user, BudgetData request, BudgetsContext budgetsContext, IAuthorizationService authorizationHandler, CancellationToken cancellationToken) =>
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

                budget.Update(request.Name, request.BeginOfPeriod, request.Limit.HasValue ? Money.FromKopecks(request.Limit.Value) : null);

                await budgetsContext.SaveChangesAsync(cancellationToken);

                return Results.NoContent();
            })
            .WithName("UpdateBudget")
            .WithTags("Budgets")
            .WithSummary("Update budget")
            .WithDescription("Updates budget by id. Requires edit permission on the budget.")
            .Produces((int)HttpStatusCode.NoContent)
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.Forbidden)
            .ProducesProblem((int)HttpStatusCode.BadRequest);
    }
}
