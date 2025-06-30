using Receipts.Contracts;
using Receipts.Core.Model;
using Category = Receipts.Core.Model.Category;
using Product = Receipts.Core.Model.Product;
using Subcategory = Receipts.Core.Model.Subcategory;

namespace Receipts.Core.Mappers;

static class CheckMapper
{
    // public static CheckDto ConvertToCheckDto(this Check check) => new CheckDto()
    // {
    //     Id = check.Id,
    //     PurchaseDate = check.PurchaseDate,
    //     AddedDate = check.AddedDate,
    // };

    public static CheckDto ConvertToCheckList(this Check check)
    {
        return new CheckDto
        {
            Id = check.Id,
            // Fd = check.Fd,
            // Fn = check.Fn,
            // Fp = check.Fp,
            AddedDate = check.AddedDate,
            PurchaseDate = check.PurchaseDate,
            Categories = ConvertToCategories(check.Products)
        };
    }

    public static IReadOnlyList<Receipts.Contracts.Category> ConvertToCategories(
        this IEnumerable<Product> products)
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

    public static Receipts.Contracts.Category ConvertToCategory(
            this IGrouping<Category,
                IGrouping<Subcategory, Product>> categories) =>
        new()
        {
            Id = categories.Key.Id,
            Name = categories.Key.Name,
            Subcategories = (from subcategory in categories
                orderby subcategory.Sum(product => product.Sum) descending
                select subcategory.ConvertToSubcategory()).ToList()
        };

    public static Receipts.Contracts.Subcategory ConvertToSubcategory(
        this IGrouping<Subcategory, Product> subcategories)
    {
        return new() 
        {
            Id = subcategories.Key.Id,
            Name = subcategories.Key.Name,
            Products = (from product in subcategories
                    orderby product.Sum descending
                    select new Receipts.Contracts.Product
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