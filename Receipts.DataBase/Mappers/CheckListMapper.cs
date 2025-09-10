using System.Linq.Expressions;
using Receipts.Core.Model;
using DBCheck = Receipts.DataBase.Entities.Check;
using DBProduct = Receipts.DataBase.Entities.Product;
using DBSubcategory = Receipts.DataBase.Entities.Subcategory;
using DBCategory = Receipts.DataBase.Entities.Category;

namespace Receipts.DataBase.Mappers;

internal static class CheckListMapper
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
    
    public static readonly Expression<Func<DBCheck, Check>> ConvertToCheckExpression = dbCheck => new()
    {
        Id = dbCheck.Id, 
        AddedDate = dbCheck.AddedDate, 
        PurchaseDate = dbCheck.PurchaseDate, 
        Fd = dbCheck.Fd,
        Fn = dbCheck.Fn, 
        Fp = dbCheck.Fp, 
        S = dbCheck.S,
        Products = dbCheck.Products.Select(dbProduct => 
            new Product (
                dbProduct.Id, 
                dbProduct.Name,
                dbProduct.Quantity, 
                dbProduct.Price, 
                dbProduct.Sum, 
                new Subcategory(
                    dbProduct.Subcategory.Id, 
                    dbProduct.Subcategory.Name, 
                    new Category(dbProduct.Subcategory.Category.Id, dbProduct.Subcategory.Category.Name)
                    ))
            ).ToList()
    };
}