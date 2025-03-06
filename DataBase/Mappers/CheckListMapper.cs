using Core.Model.ChecksList;
using DBCheck = DataBase.Entities.Check;
using DBProduct = DataBase.Entities.Product;
using DBSubcategory = DataBase.Entities.Subcategory;
using DBCategory = DataBase.Entities.Category;

namespace DataBase.Mappers;

internal sealed class DefaultSubcategoryComparer : IEqualityComparer<DBSubcategory>
{
    public bool Equals(DBSubcategory? x, DBSubcategory? y)
    {
        if (x is null || y is null) return false;
        if (ReferenceEquals(x, y)) return true;

        return x.Id == y.Id;
    }

    public int GetHashCode(DBSubcategory obj) => obj.Id.GetHashCode();
}

internal sealed class DefaultCategoryComparer : IEqualityComparer<DBCategory>
{
    public bool Equals(DBCategory? x, DBCategory? y)
    {
        if (x is null || y is null) return false;
        if (ReferenceEquals(x, y)) return true;

        return x.Id == y.Id;
    }

    public int GetHashCode(DBCategory obj) => obj.Id.GetHashCode();
}
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
            .GroupBy(product => product.Subcategory, new DefaultSubcategoryComparer())
            .GroupBy(subcategoryGroup => subcategoryGroup.Key.Category, new DefaultCategoryComparer())
            .Select(ConvertToCategory)
            .OrderByDescending(category => category.PennySum)
            .ToList();
    }

    public static Category ConvertToCategory(this IGrouping<DBCategory, IGrouping<DBSubcategory, DBProduct>> categories)
    {
        return new Category
        {
            Id = categories.Key.Id,
            Name = categories.Key.Name,
            Subcategories = categories
                .Select(ConvertToSubcategory)
                .OrderByDescending(subcategory => subcategory.PennySum)
                .ToList()
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
                .OrderByDescending(product => product.PennySum)
                .ToList()
        };
    }
}