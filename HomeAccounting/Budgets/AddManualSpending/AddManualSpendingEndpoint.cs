using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets.AddManualSpending;
using ClientServerShared.Model;
using HomeAccounting.Budgets.Data;
using HomeAccounting.Budgets.Data.Database;
using HomeAccounting.Categories.Data;
using HomeAccounting.Users.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HomeAccounting.Budgets;
using HomeAccounting.Common.Model.ValueObjects;

namespace HomeAccounting.Budgets.AddManualSpending;

/// <summary>
/// Endpoint for adding manual spending to a budget.
/// </summary>
static class AddManualSpendingEndpoint
{
    /// <summary>
    /// Maps POST /budgets/{id}/spendings/manual. Adds manual spending; requires edit permission.
    /// </summary>
    public static void MapAddManualSpending(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "{id:guid}/spendings/manual",
                async (Guid id, ClaimsPrincipal user, AddManualSpendingRequest request, BudgetsContext budgetsContext,
                    IAuthorizationService authorizationHandler, CancellationToken cancellationToken) =>
                {
                    var budgetId = new BudgetId(id);
                    var result = await authorizationHandler.AuthorizeAsync(user, budgetId,
                        new BudgetRequirements(BudgetPermissions.Edit));
                    if (!result.Succeeded)
                    {
                        return Results.Problem(statusCode: (int)HttpStatusCode.Forbidden,
                            detail: "User does not have permission to edit this budget");
                    }

                    var budget = await budgetsContext.Budgets
                        .FirstOrDefaultAsync(b => b.Id == budgetId, cancellationToken);

                    if (budget is null)
                    {
                        return Results.NotFound();
                    }

                    var userId = new UserId(user.GetUserId());
                    var addedDate = DateTimeOffset.UtcNow;

                    budget.AddOperation(
                        Money.FromKopecks(request.Sum),
                        request.Description,
                        request.CategoryId.HasValue ? new CategoryId(request.CategoryId.Value) : null,
                        request.PurchaseDate, addedDate,
                        userId);

                    await budgetsContext.SaveChangesAsync(cancellationToken);

                    return Results.Created();
                })
            .WithName("AddManualSpending")
            .WithTags("Budgets")
            .WithSummary("Add manual spending")
            .WithDescription("Adds a manual spending entry to the budget. Requires edit permission.")
            .Produces((int)HttpStatusCode.Created)
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.Forbidden)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.InternalServerError);
    }
}

