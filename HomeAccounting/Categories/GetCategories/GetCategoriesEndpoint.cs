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

                    var categoriesQuery = context.Categories.AsNoTracking()
                        .OrderBy(c=>c.Id)
                        .Include(c => c.Children);
                    if (parentCategoryId is null)
                    {

                        var categories = await categoriesQuery
                            .Where(c => c.ParentCategoryId == null)
                            .Select(c => new CategoryDto(c.Id.Value, c.Name, c.Children.Any()))
                            .ToListAsync(cancellationToken);

                        var rootResponse = new CategoriesResponse(categories, []);
                        
                        return Results.Ok(rootResponse);
                    }
                    var parentCategory =  await categoriesQuery
                        .ThenInclude(c => c.Children)
                        .FirstOrDefaultAsync(c => c.Id == parentCategoryId, cancellationToken);
                    
                    if (parentCategory is null)
                    {
                        return Results.NotFound();
                    }

                    var categoriesPath = parentCategory.Hierarchy.CategoriesPath();
                    var pathIndexed = categoriesPath.Index().ToDictionary(
                        keySelector: (item) => item.Item,
                        elementSelector: (item) => item.Index
                        );

                    var categoriesForPathQuery = from category in categoriesQuery
                        where categoriesPath.Contains(category.Id)
                        select category;
                    var categoriesForPath = (await categoriesForPathQuery.ToListAsync(cancellationToken))
                        .OrderBy(p=>pathIndexed[p.Id])
                        .Select(c =>new CategoriesPathItem(c.Id.Value, c.Name));

                    IReadOnlyList<CategoriesPathItem> path = [
                        ..categoriesForPath,
                        new CategoriesPathItem(parentCategory.Id.Value, parentCategory.Name)
                    ];
                    
                    var response = new CategoriesResponse(
                        parentCategory.Children.Select(category=> new CategoryDto(category.Id.Value, category.Name, category.Children.Any())).ToList(),
                        path
                        );
                    
                    // var categories = from category in categoriesQuery
                    //     where category.ParentCategoryId == parentCategoryId
                    //     join childCategory in categoriesQuery
                    //         on category.Id equals childCategory.ParentCategoryId
                    //         into children
                    //     select new CategoryDto(category.Id.Value, category.Name, children.Any());

                    return Results.Ok(response);
                })
            .WithName("GetCategories")
            .WithTags("Categories")
            .WithSummary("Get categories list")
            .WithDescription("Returns categories, optionally filtered by parent category id. Use null or omit ParentId for root categories.")
            .Produces((int)HttpStatusCode.OK, typeof(CategoriesResponse))
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.InternalServerError);
    }

    /// <summary>
    /// Query parameters for GetCategories endpoint.
    /// </summary>
    public sealed record GetCategoriesQuery(int? ParentId);
}

