namespace ClientServerContracts.Api.Products;

/// <summary>
/// Query parameters for GetProducts endpoint.
/// </summary>
public sealed class GetProductsQueryParameters
{
    /// <summary>
    /// Filter string for filtering by product name. Format: FieldName[Operator]=Value or FieldName=Value (default operator is Eq).
    /// Multiple filters are separated by commas. Available field: Name (ProductField). Operators: eq, ne, cn, nc, sw, ew.
    /// </summary>
    public string? Filter { get; set; }

    /// <summary>
    /// Sorting string. Format: FieldName[Order] or FieldName (default Asc). Multiple sortings separated by commas. Available field: Name (ProductSortField).
    /// </summary>
    public string? Sorting { get; set; }

    /// <summary>
    /// Number of items to take (pagination).
    /// </summary>
    public int? Take { get; set; }

    /// <summary>
    /// Number of items to skip (pagination).
    /// </summary>
    public int? Skip { get; set; }
}
