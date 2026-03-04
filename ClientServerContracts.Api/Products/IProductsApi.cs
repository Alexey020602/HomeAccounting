using ClientServerContracts.Products.GetProductNames;
using Refit;

namespace ClientServerContracts.Api.Products;

/// <summary>
/// Refit client for public Products API endpoints (no authentication required).
/// </summary>
public interface IProductsApi
{
    /// <summary>
    /// Returns paginated list of unique product names with optional filtering and sorting by name.
    /// </summary>
    /// <param name="query">Query parameters for filtering, sorting and pagination.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Product names response with items and total count.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>200</term><description>OK - Successfully retrieved product names.</description></item>
    /// <item><term>400</term><description>Bad Request - Invalid request parameters.</description></item>
    /// <item><term>500</term><description>Internal Server Error - Server error occurred.</description></item>
    /// </list>
    /// </remarks>
    [Get("/products")]
    Task<GetProductNamesResponse> GetProductNames([Query] GetProductNamesQueryParameters query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all unique product names as CSV file.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Stream containing CSV file content. Caller is responsible for disposing the stream.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Response content type is text/csv. Possible status codes:
    /// <list type="table">
    /// <item><term>200</term><description>OK - CSV file with product names.</description></item>
    /// <item><term>500</term><description>Internal Server Error - Server error occurred.</description></item>
    /// </list>
    /// </remarks>
    [Get("/products/csv")]
    Task<Stream> GetProductsCsv(CancellationToken cancellationToken = default);
}
