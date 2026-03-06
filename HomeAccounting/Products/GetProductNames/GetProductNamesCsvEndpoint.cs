using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using ClientServerContracts.Products.GetProductNames;
using CsvHelper;
using HomeAccounting.Budgets.Data.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Products.GetProductNames;

/// <summary>
/// Endpoint for downloading unique product names as CSV (public, no auth).
/// </summary>
static class GetProductNamesCsvEndpoint
{
    /// <summary>
    /// Maps GET csv for product names. Returns all unique product names as a CSV file.
    /// </summary>
    public static void MapGetProductNamesCsv(this IEndpointRouteBuilder endpoints)
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
                    // var sb = new StringBuilder();
                    // sb.AppendLine("Name");
                    // foreach (var name in names)
                    // {
                    //     var escaped = EscapeCsvField(name);
                    //     sb.AppendLine(escaped);
                    // }
                    //
                    // var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
                    context.Response.ContentType = "text/csv; charset=utf-8";
                    context.Response.Headers.ContentDisposition = "attachment; filename=\"products.csv\"";
                    await using var writer = new StreamWriter(context.Response.Body, leaveOpen: true);
                    await using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
                    await csvWriter.WriteRecordsAsync(productsResult, cancellationToken);
                    // await csvWriter.FlushAsync();
                    // await writer.FlushAsync(cancellationToken);
                    
                    // return Results.File(stream, "text/csv", "products.csv");
                })
            .WithName("GetProductNamesCsv")
            .WithTags("Products")
            .WithSummary("Get all unique product names as CSV file")
            .WithDescription("Returns all unique product names as a CSV file with a single Name column. Public endpoint, no authentication required.")
            .Produces(StatusCodes.Status200OK);
    }

    private static async IAsyncEnumerable<T> StreamManyAsync<T>(
        this IAsyncEnumerable<T> source,
        int repeatCount,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        const int batchSize = 1000;
        var batchesCount = repeatCount / batchSize;
        var remaining = repeatCount % batchSize;
        await foreach (var item in source.WithCancellation(cancellationToken))
        {

            for(var i = 0; i < batchesCount; i++)
            {
                await Task.Yield();
                for (var j = 0; j < batchSize; j++)
                {
                    yield return item;
                }
            }

            await Task.Yield();
            for (int i = 0; i < remaining; i++)
            {
                yield return item;
            }
        }
    }
    
}
