using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets.GetBudgetSpendings;
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

                if (!await budgetsContext.Budgets.AnyAsync(b => b.Id == budgetId, cancellationToken))
                {
                    return Results.NotFound();
                }

                var operations = await budgetsContext.Budgets
                    .AsNoTracking()
                    .Where(b => b.Id == budgetId)
                    .SelectMany(b => b.Operations)
                    .ToArrayAsync(cancellationToken);

                var receipts = await budgetsContext.Receipts
                    .AsNoTracking()
                    .Where(r => r.BudgetId == budgetId)
                    .ToArrayAsync(cancellationToken);

                var operationDtos = operations
                    .Select(op => new SpendingDto(op.Id.Value, op.Description, op.Sum.Kopecks, Status.Added));

                var receiptDtos = receipts
                    .Select(r => new SpendingDto(r.Id.Value, r.Description, r.Sum.Kopecks, MapReceiptStatus(r.Status)));

                var spendings = operationDtos.Concat(receiptDtos).ToArray();
                var response = new GetBudgetSpendingsResponse(spendings);
                return Results.Ok(response);
            })
            .WithName("GetBudgetSpendings")
            .WithTags("Budgets")
            .WithSummary("Get budget spendings")
            .WithDescription("Returns list of spendings in the budget (operations and receipts). Requires read permission.")
            .Produces((int)HttpStatusCode.OK, typeof(GetBudgetSpendingsResponse))
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.Forbidden);
    }

    private static Status MapReceiptStatus(ReceiptProcessingStatus status) => status switch
    {
        ReceiptProcessingStatus.Processing => Status.InProcess,
        ReceiptProcessingStatus.Succeeded => Status.Added,
        ReceiptProcessingStatus.Failed => Status.Error,
        _ => Status.Added
    };
}
