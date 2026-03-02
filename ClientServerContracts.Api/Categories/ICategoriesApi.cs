using ClientServerContracts.Categories.GetCategories;
using ClientServerContracts.Categories.GetCategoriesTree;
using Refit;

namespace ClientServerContracts.Api.Categories;

/// <summary>
/// Query parameters for GetCategories endpoint.
/// </summary>
public sealed class GetCategoriesQueryParameters
{
    /// <summary>
    /// Optional parent category id. Use null for root categories.
    /// </summary>
    public int? ParentId { get; set; }
}

/// <summary>
/// Refit client for Categories API endpoints.
/// </summary>
// [Headers("Authorization: Bearer")]
public interface ICategoriesApi
{
    /// <summary>
    /// Returns categories, optionally filtered by parent category id. Use null or omit ParentId for root categories.
    /// </summary>
    /// <param name="query">Query parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Collection of categories.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>200</term><description>OK - Successfully retrieved categories.</description></item>
    /// <item><term>400</term><description>Bad Request - Invalid request parameters.</description></item>
    /// <item><term>500</term><description>Internal Server Error - Server error occurred.</description></item>
    /// </list>
    /// </remarks>
    [Get("/categories")]
    Task<CategoriesResponse> GetCategories([Query] GetCategoriesQueryParameters query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all categories as a hierarchical tree with parent-child structure.
    /// </summary>
    /// <returns>Collection of category trees.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>200</term><description>OK - Successfully retrieved categories tree.</description></item>
    /// <item><term>400</term><description>Bad Request - Invalid request.</description></item>
    /// <item><term>500</term><description>Internal Server Error - Server error occurred.</description></item>
    /// </list>
    /// </remarks>
    [Get("/categories/tree")]
    Task<IReadOnlyCollection<CategoryTreeDto>> GetCategoriesTree(CancellationToken cancellationToken = default);
}
