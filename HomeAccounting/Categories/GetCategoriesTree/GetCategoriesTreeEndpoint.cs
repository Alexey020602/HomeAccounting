using System.Net;
using ClientServerContracts.Categories.GetCategories;
using HomeAccounting.Categories.Data;
using HomeAccounting.Categories.Data.DataBase;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Categories.GetCategoriesTree;

/// <summary>
/// Endpoint for retrieving categories as a hierarchical tree.
/// </summary>
static class GetCategoriesTreeEndpoint
{
    /// <summary>
    /// Maps GET /categories/tree. Returns all categories as a tree structure.
    /// </summary>
    public static void MapGetCategoriesTree(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/tree",
                async (CategoriesContext context, CancellationToken cancellationToken) =>
                {
                var allCategories = await context.Categories
                    .AsNoTracking()
                    .Select(c => new CategoryInfo
                    {
                        Id = c.Id.Value,
                        Name = c.Name,
                        ParentCategoryId = c.ParentCategoryId.HasValue ? c.ParentCategoryId.Value.Value : null
                    })
                    .ToListAsync(cancellationToken);

                var rootCategories = allCategories.Where(c => c.ParentCategoryId == null).ToList();

                CategoryTreeDto BuildTree(CategoryInfo category)
                {
                    var children = allCategories
                        .Where(c => c.ParentCategoryId == category.Id)
                        .Select(BuildTree)
                        .ToList();

                    return new CategoryTreeDto(
                        category.Id,
                        category.Name,
                        category.ParentCategoryId,
                        children);
                }

                var tree = rootCategories.Select(BuildTree).ToList();

                return Results.Ok(tree);
                })
            .WithName("GetCategoriesTree")
            .WithTags("Categories")
            .WithSummary("Get categories tree")
            .WithDescription("Returns all categories as a hierarchical tree with parent-child structure.")
            .Produces((int)HttpStatusCode.OK, typeof(IReadOnlyCollection<CategoryTreeDto>))
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.InternalServerError);
    }

    private sealed class CategoryInfo
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public int? ParentCategoryId { get; init; }
    }
}

