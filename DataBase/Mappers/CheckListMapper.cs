using Core.Model.ChecksList;
using DBCheck = DataBase.Entities.Check;
using DBProduct = DataBase.Entities.Product;
using DBSubcategory = DataBase.Entities.Subcategory;
using DBCategory = DataBase.Entities.Category;

namespace DataBase.Mappers;

public static class CheckListMapper
{
    public static Check ConvertToCheckList(this DBCheck check)
    {
        return new Check
        {
            Id = check.Id,
            AddedDate = check.AddedDate,
            PurchaseDate = check.PurchaseDate,
            Categories = ConvertToCategories(check.Products)
        };
    }

    public static IReadOnlyList<Category> ConvertToCategories(this IEnumerable<DBProduct> products)
    {
        var categories = from product in products
            group product by product.Subcategory
            into subcategoryGroup
            group subcategoryGroup by subcategoryGroup.Key.Category
            into categoryGroup
            orderby categoryGroup
                .Sum(subcategory => subcategory.Sum(p => p.Sum))
            descending 
            select categoryGroup.ConvertToCategory();

        return categories.OrderByDescending(category => category.PennySum).ToList();
    }

    public static Category
        ConvertToCategory(this IGrouping<DBCategory, IGrouping<DBSubcategory, DBProduct>> categories) =>
        new()
        {
            Id = categories.Key.Id,
            Name = categories.Key.Name,
            Subcategories = (from subcategory in categories
                orderby subcategory.Sum(product => product.Sum) descending
                select subcategory.ConvertToSubcategory()).ToList()
        };

    public static Subcategory ConvertToSubcategory(this IGrouping<DBSubcategory, DBProduct> subcategories)
    {
        return new Subcategory
        {
            Id = subcategories.Key.Id,
            Name = subcategories.Key.Name,
            Products = (from product in subcategories
                    orderby product.Sum descending 
                        select new Product
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Quantity = product.Quantity,
                        PennySum = product.Sum
                    })
                .ToList()
        };
    }
}