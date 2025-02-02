using Core.Model.ChecksList;
using DBCheck = DataBase.Entities.Check;
using DBProduct = DataBase.Entities.Product;
using DBSubcategory = DataBase.Entities.Subcategory;
using DBCategory = DataBase.Entities.Category;

namespace Core.Mappers;

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
        return products
            .GroupBy(product => product.Subcategory)
            .GroupBy(subcategoryGroup => subcategoryGroup.Key.Category)
            .Select(ConvertToCategory)
            .ToList();
    }

    public static Category ConvertToCategory(this IGrouping<DBCategory, IGrouping<DBSubcategory, DBProduct>> categories)
    {
        return new Category
        {
            Id = categories.Key.Id,
            Name = categories.Key.Name,
            Subcategories = categories.Select(ConvertToSubcategory).ToList()
        };
    }

    public static Subcategory ConvertToSubcategory(this IGrouping<DBSubcategory, DBProduct> subcategories)
    {
        return new Subcategory
        {
            Id = subcategories.Key.Id,
            Name = subcategories.Key.Name,
            Products = subcategories
                .Select(product => new Product
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