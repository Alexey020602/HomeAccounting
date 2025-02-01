using Core.Mappers;
using Core.Model.ChecksList;
using Core.Services;
using DataBase;
using Microsoft.EntityFrameworkCore;
using DBProduct = DataBase.Entities.Product;
using DBSubcategory = DataBase.Entities.Subcategory;
using DBCategory = DataBase.Entities.Category;
namespace Core;

public class ReportUseCase(ApplicationContext context): IReportUseCase
{
    public async Task<IReadOnlyList<Check>> GetChecksAsync(int skip = 0, int take = 100)
    {
        var checks = await context.Checks
            .Include(c => c.Products)
            .ThenInclude(p => p.Subcategory)
            .ThenInclude(sub => sub.Category)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
        
        
        return checks.ConvertAll(check => new Check()
        {
            Id = check.Id,
            AddedDate = check.AddedDate,
            PurchaseDate = check.PurchaseDate,
            Categories = ConvertToCategories(check.Products),
        });
    }

    private static IReadOnlyList<Category> ConvertToCategories(IReadOnlyList<DBProduct> products) => products
        .GroupBy(product => product.Subcategory)
        .GroupBy(subcategoryGroup => subcategoryGroup.Key.Category)
        .Select(ConvertToCategory)
        .ToList();
    private static Category ConvertToCategory(IGrouping<DBCategory, IGrouping<DBSubcategory, DBProduct>> categories) =>
        new Category()
        {
            Id = categories.Key.Id,
            Name = categories.Key.Name,
            Subcategories = categories.Select(ConvertToSubcategory).ToList(),
        };
    private static Subcategory ConvertToSubcategory(IGrouping<DBSubcategory, DBProduct> subcategories) =>
        new Subcategory()
        {
            Id = subcategories.Key.Id,
            Name = subcategories.Key.Name,
            Products = subcategories
                .Select(product => new Product()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    PennySum = product.Sum,
                })
                .ToList(),
        };
}