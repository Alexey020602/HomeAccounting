using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets.GetBudgetSpendings;
using ClientServerShared.Model;
using HomeAccounting.Budgets.Data;
using HomeAccounting.Budgets.Data.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HomeAccounting.Budgets;

namespace HomeAccounting.Budgets.GetBudgetSpendings;

/// <summary>
/// Endpoint for retrieving spendings of a budget.
/// </summary>
static class GetBudgetSpendingsEndpoint
{
    /// <summary>
    /// Maps GET /budgets/{id}/spendings. Returns list of spendings; requires read permission.
    /// </summary>
    public static void MapGetBudgetSpendings(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "{id:guid}/spendings",
            async (Guid id, ClaimsPrincipal user, BudgetsContext budgetsContext, IAuthorizationService authorizationHandler, CancellationToken cancellationToken) =>
            {
                var budgetId = new BudgetId(id);
                var result = await authorizationHandler.AuthorizeAsync(user, budgetId, new BudgetRequirements(BudgetPermissions.Read));
                if (!result.Succeeded)
                {
                    return Results.Problem(statusCode: (int)HttpStatusCode.Forbidden, detail: "User does not have permission to read this budget");
                }

                if (!await budgetsContext.Budgets.AnyAsync(budget => budget.Id == budgetId, cancellationToken: cancellationToken))
                {
                    return Results.NotFound();
                }

                var spendingsQuery = from budget in budgetsContext.Budgets.AsNoTracking()
                    where budget.Id == budgetId
                    from spending in budget.Spendings
                    select spending;

                var loadedSpendings = await spendingsQuery.ToArrayAsync(cancellationToken);

                var spendings = loadedSpendings
                    .Select(spending => new SpendingDto(
                        spending.Id.Value,
                        spending.Description,
                        spending.Sum.Kopecks,
                        MapToContractStatus(spending)))
                    .ToArray();

                var response = new GetBudgetSpendingsResponse(spendings);
                return Results.Ok(response);
            })
            .WithName("GetBudgetSpendings")
            .WithTags("Budgets")
            .WithSummary("Get budget spendings")
            .WithDescription("Returns list of spendings in the budget. Requires read permission.")
            .Produces((int)HttpStatusCode.OK, typeof(GetBudgetSpendingsResponse))
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.Forbidden);
    }

    private static Status MapToContractStatus(Spending spending)
    {
        if (spending is ReceiptSpending receipt)
            return receipt.Status switch
            {
                ReceiptProcessingStatus.Processing => Status.InProcess,
                ReceiptProcessingStatus.Succeeded => Status.Added,
                ReceiptProcessingStatus.Failed => Status.Error,
                _ => Status.Added
            };
        return Status.Added;
    }
}
