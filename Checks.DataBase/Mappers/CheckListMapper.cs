using Checks.Core.Model;
using Shared.Model;
using Shared.Model.Checks;
using DBCheck = Checks.DataBase.Entities.Check;
using DBProduct = Checks.DataBase.Entities.Product;
using DBSubcategory = Checks.DataBase.Entities.Subcategory;
using DBCategory = Checks.DataBase.Entities.Category;

namespace Checks.DataBase.Mappers;

static class CheckListMapper
{
    public static Category ConvertToCategory(this DBCategory dbCategory) => new(dbCategory.Id, dbCategory.Name);

    public static Subcategory ConvertToSubcategory(this DBSubcategory dbSubcategory) => new(dbSubcategory.Id,
        dbSubcategory.Name, dbSubcategory.Category.ConvertToCategory());

    public static Product ConvertToProduct(this DBProduct dbProduct) => new(dbProduct.Id, dbProduct.Name,
        dbProduct.Quantity, dbProduct.Price, dbProduct.Sum, dbProduct.Subcategory.ConvertToSubcategory());

    public static IEnumerable<Product> ConvertToProducts(this IEnumerable<DBProduct> dbProducts) =>
        dbProducts.Select(ConvertToProduct);

    public static List<Product> ConvertToProducts(this List<DBProduct> dbProduct) =>
        dbProduct.ConvertAll(ConvertToProduct);

    public static Check ConvertToCheck(this DBCheck dbCheck) => new()
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