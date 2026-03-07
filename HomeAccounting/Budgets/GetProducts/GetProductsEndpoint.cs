using System.Net;
using ClientServerContracts.Budgets.GetProducts;
using HomeAccounting.Budgets.Data.Database;
using HomeAccounting.Common.Application.Paging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Budgets.GetProducts;

/// <summary>
/// Endpoint for retrieving unique product names list (public, no auth).
/// </summary>
static class GetProductsEndpoint
{
    /// <summary>
    /// Maps GET for product names list. Returns paginated list with filtering and sorting by name.
    /// </summary>
    public static void MapGetProducts(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "",
                async (
                    [AsParameters] GetProductsRequest request,
                    BudgetsContext budgetsContext,
                    CancellationToken cancellationToken) =>
                {
                    var baseQuery = budgetsContext.Receipts
                        .AsNoTracking()
                        .SelectMany(r => r.Products)
                        .GroupBy(p => p.Name)
                        .Select(g => new ProductNameRow(g.Key));

                    if (request.Filter is not null && request.Filter.ParseFilter<ProductField>() is var filters)
                    {
                        baseQuery = baseQuery.ApplyFilters(filters);
                    }

                    if (request.Sorting.ParseSorting<ProductSortField>() is var sortings)
                    {
                        baseQuery = baseQuery.ApplySortings(sortings);
                    }

                    var totalCount = await baseQuery.CountAsync(cancellationToken);

                    if (request.Skip.HasValue)
                    {
                        baseQuery = baseQuery.Skip(request.Skip.Value);
                    }

                    if (request.Take.HasValue)
                    {
                        baseQuery = baseQuery.Take(request.Take.Value);
                    }

                    var rows = await baseQuery.ToArrayAsync(cancellationToken);
                    var items = rows.Select(r => new ProductDto(r.Name)).ToArray();
                    var response = new GetProductsResponse(items, totalCount);
                    return Results.Ok(response);
                })
            .WithName("GetProducts")
            .WithTags("Products")
            .WithSummary("Get unique product names list")
            .WithDescription("Returns paginated list of unique product names with optional filtering and sorting by name. Public endpoint, no authentication required.")
            .Produces((int)HttpStatusCode.OK, typeof(GetProductsResponse))
            .ProducesProblem((int)HttpStatusCode.InternalServerError);
    }

    private sealed record ProductNameRow(string Name);
}
