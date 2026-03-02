using System.Globalization;
using CsvHelper;
using HomeAccounting.Categories.Data.DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Categories.GetCategoriesCsv;

internal static class GetCategoriesCsvEndpoint
{
    extension(IEndpointRouteBuilder endpoints)
    {
        public void MapGetCategoriesCsv() => endpoints.MapGet(
            "csv",
            async (CategoriesContext categoriesContext, CancellationToken cancellationToken) =>
            {
                var categories = await categoriesContext.Categories
                    .OrderBy(c => c.Id)
                    .Select(c=>new CategoryRow(
                        c.Id.Value,
                        c.Name,
                        c.ParentCategoryId.HasValue ?  c.ParentCategoryId.Value.Value : null)
                    )
                    .ToListAsync(cancellationToken);
                
                var stream = new MemoryStream();
                await using var writer = new StreamWriter(stream, leaveOpen: true);
                await using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
                
                await csvWriter.WriteRecordsAsync(categories, cancellationToken);

                await csvWriter.FlushAsync();
                await writer.FlushAsync(cancellationToken);
                stream.Position = 0;
                
                return Results.File(stream, "text/csv", "categories.csv");
            }
        )
        .WithName("GetCategoriesCsv")
        .WithTags("Categories")
        .WithSummary("Gets all categories as CSV file")
        .WithDescription("Gets all categories as CSV file")
        .Produces(StatusCodes.Status200OK);
    }

    private sealed record CategoryRow(int Id, string Name, int? ParentId);
}
