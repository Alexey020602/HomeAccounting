using Checks.Contracts;
using Checks.Core.Model;

namespace Checks.Core.Mappers;

static class CheckMapper
{
    // public static CheckDto ConvertToCheckDto(this Check check) => new CheckDto()
    // {
    //     Id = check.Id,
    //     PurchaseDate = check.PurchaseDate,
    //     AddedDate = check.AddedDate,
    // };

    public static CheckDto ConvertToCheckList(this Checks.Core.Model.Check check)
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

    public static IReadOnlyList<Checks.Contracts.Category> ConvertToCategories(
        this IEnumerable<Checks.Core.Model.Product> products)
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

    public static Contracts.Category ConvertToCategory(
            this IGrouping<Checks.Core.Model.Category,
                IGrouping<Checks.Core.Model.Subcategory, Checks.Core.Model.Product>> categories) =>
        new()
        {
            Id = categories.Key.Id,
            Name = categories.Key.Name,
            Subcategories = (from subcategory in categories
                orderby subcategory.Sum(product => product.Sum) descending
                select subcategory.ConvertToSubcategory()).ToList()
        };

    public static Contracts.Subcategory ConvertToSubcategory(
        this IGrouping<Checks.Core.Model.Subcategory, Checks.Core.Model.Product> subcategories)
    {
        return new() 
        {
            Id = subcategories.Key.Id,
            Name = subcategories.Key.Name,
            Products = (from product in subcategories
                    orderby product.Sum descending
                    select new Contracts.Product
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