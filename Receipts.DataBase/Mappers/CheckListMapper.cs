using Receipts.Core.Model;
using Shared.Model;
using Shared.Model.Checks;
using DBCheck = Receipts.DataBase.Entities.Check;
using DBProduct = Receipts.DataBase.Entities.Product;
using DBSubcategory = Receipts.DataBase.Entities.Subcategory;
using DBCategory = Receipts.DataBase.Entities.Category;

namespace Receipts.DataBase.Mappers;

static class CheckListMapper
{
    public static Category ConvertToCategory(this Entities.Category dbCategory) => new(dbCategory.Id, dbCategory.Name);

    public static Subcategory ConvertToSubcategory(this Entities.Subcategory dbSubcategory) => new(dbSubcategory.Id,
        dbSubcategory.Name, dbSubcategory.Category.ConvertToCategory());

    public static Product ConvertToProduct(this Entities.Product dbProduct) => new(dbProduct.Id, dbProduct.Name,
        dbProduct.Quantity, dbProduct.Price, dbProduct.Sum, dbProduct.Subcategory.ConvertToSubcategory());

    public static IEnumerable<Product> ConvertToProducts(this IEnumerable<Entities.Product> dbProducts) =>
        dbProducts.Select(ConvertToProduct);

    public static List<Product> ConvertToProducts(this List<Entities.Product> dbProduct) =>
        dbProduct.ConvertAll(ConvertToProduct);

    public static Check ConvertToCheck(this Entities.Check dbCheck) => new()
    {
        Id = dbCheck.Id, 
        AddedDate = dbCheck.AddedDate, 
        PurchaseDate = dbCheck.PurchaseDate, 
        Fd = dbCheck.Fd,
        Fn = dbCheck.Fn, 
        Fp = dbCheck.Fp, 
        S = dbCheck.S,
        Products = dbCheck.Products.ConvertToProducts()
    };
}