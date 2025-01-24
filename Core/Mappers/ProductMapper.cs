using Core.Model;

namespace Core.Mappers;

public static class ProductMapper
{
    public static Product ConvertToProduct(this DataBase.Entities.Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Quantity = product.Quantity,
        Sum = product.Sum,
        Subcategory = product.Subcategory.ConvertToSubcategory(),
    };
    
    public static IEnumerable<Product> ConvertToProducts(this IEnumerable<DataBase.Entities.Product> products) => products.Select(ConvertToProduct);
}