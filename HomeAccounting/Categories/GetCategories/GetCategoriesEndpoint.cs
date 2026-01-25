using System.Net;
using ClientServerContracts.Categories.GetCategories;
using HomeAccounting.Categories.Data;
using HomeAccounting.Categories.Data.DataBase;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Categories.GetCategories;

static class GetCategoriesEndpoint
{
    public static void MapGetCategories(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "",
            async (int? parentId, CategoriesContext context, CancellationToken cancellationToken) =>
            {
                CategoryId? parentCategoryId = parentId.HasValue ? new CategoryId(parentId.Value) : null;
                
                var query = context.Categories.AsNoTracking();
                var categories = from category in query
                    where category.ParentCategoryId == parentCategoryId
                    join childCategory in query 
                        on category.Id equals childCategory.ParentCategoryId 
                        into children
                    select new CategoryDto(category.Id.Value, category.Name, children.Any());
                // if (parentId.HasValue)
                // {
                //     
                //     query = query.Where(c => c.ParentCategoryId == parentCategoryId);
                // }
                // else
                // {
                //     query = query.Where(c => c.ParentCategoryId == null);
                // }

                // var categories = await query
                //     .Select(c => new CategoryDto(
                //         c.Id.Value,
                //         c.Name,
                //         c.ParentCategoryId.HasValue ? c.ParentCategoryId.Value.Value : null))
                //     .ToListAsync();

                return Results.Ok(await categories.ToListAsync(cancellationToken));
            })
            .Produces((int)HttpStatusCode.OK, typeof(IReadOnlyCollection<CategoryDto>))
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.InternalServerError);
    }
}

