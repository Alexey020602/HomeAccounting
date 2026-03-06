using HomeAccounting.Products.GetProductNames;

namespace HomeAccounting.Products;

internal static class ProductsEndpoints
{
    public static void MapProductsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGetProductNames();
        endpoints.MapGetProductNamesCsv();
    }
}
