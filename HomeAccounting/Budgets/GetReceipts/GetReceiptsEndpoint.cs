using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets.GetReceipts;
using HomeAccounting.Budgets.Data;
using HomeAccounting.Budgets.Data.Database;
using HomeAccounting.Users.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeAccounting.Budgets;

namespace HomeAccounting.Budgets.GetReceipts;

/// <summary>
/// Endpoint for retrieving receipts of a budget.
/// </summary>
static class GetReceiptsEndpoint
{
    /// <summary>
    /// Maps GET /budgets/{id}/receipts. Returns list of receipts with filtering, sorting and pagination; requires read permission.
    /// </summary>
    public static void MapGetReceipts(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "{id:guid}/receipts",
            async (
                Guid id,
                [AsParameters] GetReceiptsRequest request,
                ClaimsPrincipal user,
                BudgetsContext budgetsContext,
                IAuthorizationService authorizationHandler,
                CancellationToken cancellationToken) =>
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

                // Load receipts from budget
                // Include Products for Sum calculation (Sum is computed property that depends on Products)
                var sortBy = request.SortBy ?? "PurchaseDate";
                var needsProductsForSorting = sortBy == "Sum";
                
                var receiptsQuery = budgetsContext.Receipts
                    .AsNoTracking()
                    .Where(r => r.BudgetId == budgetId)
                    .Include(r => r.Products)
                    .AsQueryable();

                // Apply filters
                if (request.StartDate.HasValue)
                {
                    receiptsQuery = receiptsQuery.Where(r => r.PurchaseDate >= request.StartDate.Value);
                }

                if (request.EndDate.HasValue)
                {
                    receiptsQuery = receiptsQuery.Where(r => r.PurchaseDate <= request.EndDate.Value);
                }

                if (request.UserId.HasValue)
                {
                    var userId = new UserId(request.UserId.Value);
                    receiptsQuery = receiptsQuery.Where(r => r.UserId == userId);
                }

                if (request.Status.HasValue)
                {
                    var status = (ReceiptProcessingStatus)request.Status.Value;
                    receiptsQuery = receiptsQuery.Where(r => r.Status == status);
                }

                // Count total before pagination
                var totalCount = await receiptsQuery.CountAsync(cancellationToken);

                // Apply sorting
                var sortDescending = request.SortDescending ?? true;

                // For other sort fields, use database sorting
                receiptsQuery = sortBy switch
                {
                    "Sum" => sortDescending 
                    ? receiptsQuery.OrderByDescending(r => r.Sum)
                        : receiptsQuery.OrderBy(r => r.Sum),
                    "PurchaseDate" => sortDescending
                        ? receiptsQuery.OrderByDescending(r => r.PurchaseDate)
                        : receiptsQuery.OrderBy(r => r.PurchaseDate),
                    "CreatedAt" => sortDescending
                        ? receiptsQuery.OrderByDescending(r => r.CreatedAt)
                        : receiptsQuery.OrderBy(r => r.CreatedAt),
                    "CompletedAt" => sortDescending
                        ? receiptsQuery.OrderByDescending(r => r.CompletedAt ?? DateTimeOffset.MaxValue)
                        : receiptsQuery.OrderBy(r => r.CompletedAt ?? DateTimeOffset.MinValue),
                    "PurchasePlace" => sortDescending
                        ? receiptsQuery.OrderByDescending(r => r.PurchasePlace)
                        : receiptsQuery.OrderBy(r => r.PurchasePlace),
                    _ => receiptsQuery.OrderByDescending(r => r.PurchaseDate)
                };

                // Apply pagination
                if (request.Skip.HasValue)
                {
                    receiptsQuery = receiptsQuery.Skip(request.Skip.Value);
                }

                if (request.Take.HasValue)
                {
                    receiptsQuery = receiptsQuery.Take(request.Take.Value);
                }

                // Execute query and map to DTOs
                var receipts = await receiptsQuery.ToArrayAsync(cancellationToken);
                var receiptDtos = receipts.Select(r => new ReceiptDto(
                    r.Id.Value,
                    r.Sum.ToString(),
                    r.PurchaseDate,
                    r.CreatedAt,
                    r.CompletedAt,
                    r.UserId.Value,
                    r.PurchasePlace,
                    MapReceiptStatus(r.Status),
                    r.LastErrorMessage,
                    r.Fn.Value,
                    r.Fd.Value,
                    r.Fp.Value
                )).ToArray();

                var response = new GetReceiptsResponse(receiptDtos, totalCount);
                return Results.Ok(response);
            })
            .WithName("GetReceipts")
            .WithTags("Budgets")
            .WithSummary("Get budget receipts")
            .WithDescription("Returns list of receipts in the budget with optional filtering by date range, user, and status. Supports sorting and pagination. Requires read permission.")
            .Produces((int)HttpStatusCode.OK, typeof(GetReceiptsResponse))
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.Forbidden);
    }

    private static ReceiptStatus MapReceiptStatus(ReceiptProcessingStatus status) => status switch
    {
        ReceiptProcessingStatus.Processing => ReceiptStatus.Processing,
        ReceiptProcessingStatus.Succeeded => ReceiptStatus.Succeeded,
        ReceiptProcessingStatus.Failed => ReceiptStatus.Failed,
        _ => throw new ArgumentException($"Unknown receipt status: {status}")
    };
}
