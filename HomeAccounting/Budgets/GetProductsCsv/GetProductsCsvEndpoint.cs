using System.Globalization;
using ClientServerContracts.Budgets.GetProducts;
using CsvHelper;
using HomeAccounting.Budgets.Data.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Budgets.GetProductsCsv;

/// <summary>
/// Endpoint for downloading unique product names as CSV (public, no auth).
/// </summary>
static class GetProductsCsvEndpoint
{
    /// <summary>
    /// Maps GET csv for product names. Returns all unique product names as a CSV file.
    /// </summary>
    public static void MapGetProductsCsv(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "csv",
                async (BudgetsContext budgetsContext, HttpContext context, CancellationToken cancellationToken) =>
                {
                    var productsResult = budgetsContext.Receipts
                        .AsNoTracking()
                        .SelectMany(r => r.Products)
                        .GroupBy(p => p.Name)
                        .Select(g => new ProductDto(g.Key))
                        .ToAsyncEnumerable();

                    context.Response.ContentType = "text/csv; charset=utf-8";
                    context.Response.Headers.ContentDisposition = "attachment; filename=\"products.csv\"";
                    await using var writer = new StreamWriter(context.Response.Body, leaveOpen: true);
                    await using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
                    await csvWriter.WriteRecordsAsync(productsResult, cancellationToken);
                })
            .WithName("GetProductsCsv")
            .WithTags("Products")
            .WithSummary("Get all unique product names as CSV file")
            .WithDescription("Returns all unique product names as a CSV file with a single Name column. Public endpoint, no authentication required.")
            .Produces(StatusCodes.Status200OK);
    }
}
