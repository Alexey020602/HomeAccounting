using System.Net;
using ClientServerContracts.Categories.GetCategories;
using HomeAccounting.Categories.Data;
using HomeAccounting.Categories.Data.DataBase;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Categories.GetCategories;

/// <summary>
/// Endpoint for retrieving categories list, optionally filtered by parent category.
/// </summary>
static class GetCategoriesEndpoint
{
    /// <summary>
    /// Maps GET /categories with optional parent filter. Returns flat list of categories.
    /// </summary>
    public static void MapGetCategories(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "",
                async ( [AsParameters] GetCategoriesQuery query, CategoriesContext context, CancellationToken cancellationToken) =>
                {
                    CategoryId? parentCategoryId = query.ParentId.HasValue ? new CategoryId(query.ParentId.Value) : null;

                    var categoriesQuery = context.Categories.AsNoTracking();
                    var categories = from category in categoriesQuery
                        where category.ParentCategoryId == parentCategoryId
                        join childCategory in categoriesQuery
                            on category.Id equals childCategory.ParentCategoryId
                            into children
                        select new CategoryDto(category.Id.Value, category.Name, children.Any());

                    return Results.Ok(await categories.ToListAsync(cancellationToken));
                })
            .WithName("GetCategories")
            .WithTags("Categories")
            .WithSummary("Get categories list")
            .WithDescription("Returns categories, optionally filtered by parent category id. Use null or omit ParentId for root categories.")
            .Produces((int)HttpStatusCode.OK, typeof(IReadOnlyCollection<CategoryDto>))
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.InternalServerError);
    }

    /// <summary>
    /// Query parameters for GetCategories endpoint.
    /// </summary>
    public sealed record GetCategoriesQuery(int? ParentId);
}

