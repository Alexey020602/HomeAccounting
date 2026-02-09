using System.Net;
using System.Security.Claims;
using ClientServerContracts.Budgets.GetOperations;
using HomeAccounting.Budgets.Data;
using HomeAccounting.Budgets.Data.Database;
using HomeAccounting.Categories.Data;
using HomeAccounting.Users.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeAccounting.Budgets;

namespace HomeAccounting.Budgets.GetOperations;

/// <summary>
/// Endpoint for retrieving operations (manual spendings) of a budget.
/// </summary>
static class GetOperationsEndpoint
{
    /// <summary>
    /// Maps GET /budgets/{id}/operations. Returns list of operations with filtering, sorting and pagination; requires read permission.
    /// </summary>
    public static void MapGetOperations(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "{id:guid}/operations",
            async (
                Guid id,
                [AsParameters] GetOperationsRequest request,
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

                // Load operations from budget
                var operationsQuery = budgetsContext.Budgets
                    .AsNoTracking()
                    .Where(b => b.Id == budgetId)
                    .SelectMany(b => b.Operations)
                    .AsQueryable();

                // Apply filters
                if (request.StartDate.HasValue)
                {
                    operationsQuery = operationsQuery.Where(op => op.PurchaseDate >= request.StartDate.Value);
                }

                if (request.EndDate.HasValue)
                {
                    operationsQuery = operationsQuery.Where(op => op.PurchaseDate <= request.EndDate.Value);
                }

                if (request.CategoryId.HasValue)
                {
                    var categoryId = new CategoryId(request.CategoryId.Value);
                    operationsQuery = operationsQuery.Where(op => op.CategoryId == categoryId);
                }

                if (request.UserId.HasValue)
                {
                    var userId = new UserId(request.UserId.Value);
                    operationsQuery = operationsQuery.Where(op => op.UserId == userId);
                }

                // Count total before pagination
                var totalCount = await operationsQuery.CountAsync(cancellationToken);

                // Apply sorting
                var sortBy = request.SortBy ?? "PurchaseDate";
                var sortDescending = request.SortDescending ?? true;

                operationsQuery = sortBy switch
                {
                    "PurchaseDate" => sortDescending
                        ? operationsQuery.OrderByDescending(op => op.PurchaseDate)
                        : operationsQuery.OrderBy(op => op.PurchaseDate),
                    "AddedDate" => sortDescending
                        ? operationsQuery.OrderByDescending(op => op.AddedDate)
                        : operationsQuery.OrderBy(op => op.AddedDate),
                    "Sum" => sortDescending
                        ? operationsQuery.OrderByDescending(op => op.Sum.Kopecks)
                        : operationsQuery.OrderBy(op => op.Sum.Kopecks),
                    "Description" => sortDescending
                        ? operationsQuery.OrderByDescending(op => op.Description)
                        : operationsQuery.OrderBy(op => op.Description),
                    _ => operationsQuery.OrderByDescending(op => op.PurchaseDate)
                };

                // Apply pagination
                if (request.Skip.HasValue)
                {
                    operationsQuery = operationsQuery.Skip(request.Skip.Value);
                }

                if (request.Take.HasValue)
                {
                    operationsQuery = operationsQuery.Take(request.Take.Value);
                }

                // Execute query and map to DTOs
                var operations = await operationsQuery.ToArrayAsync(cancellationToken);
                var operationDtos = operations.Select(op => new OperationDto(
                    op.Id.Value,
                    op.Description,
                    op.Sum.Kopecks,
                    op.PurchaseDate,
                    op.AddedDate,
                    op.UserId.Value,
                    op.CategoryId?.Value
                )).ToArray();

                var response = new GetOperationsResponse(operationDtos, totalCount);
                return Results.Ok(response);
            })
            .WithName("GetOperations")
            .WithTags("Budgets")
            .WithSummary("Get budget operations")
            .WithDescription("Returns list of operations in the budget with optional filtering by date range, category, and user. Supports sorting and pagination. Requires read permission.")
            .Produces((int)HttpStatusCode.OK, typeof(GetOperationsResponse))
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.Forbidden);
    }
}
