namespace ClientServerContracts.Budgets.GetProducts;

/// <summary>
/// Response containing list of unique product names with total count.
/// </summary>
/// <param name="Items">Page of product DTOs.</param>
/// <param name="TotalCount">Total number of unique product names.</param>
public sealed record GetProductsResponse(ProductDto[] Items, int TotalCount);
