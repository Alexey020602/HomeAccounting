using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Receipts.Contracts;
using Receipts.Core;
using Receipts.Core.Model;
using Receipts.DataBase.Mappers;
using Category = Receipts.DataBase.Entities.Category;
using Product = Receipts.DataBase.Entities.Product;
using Subcategory = Receipts.DataBase.Entities.Subcategory;

namespace Receipts.DataBase;

static class ReportRequestExtensions
{
    public static Expression<Func<Entities.Check, bool>> GetPredicate(this GetChecksQuery request) =>
        check => (!request.Range.Start.HasValue || check.PurchaseDate >= request.Range.Start.Value.ToUniversalTime())
                 && (!request.Range.End.HasValue || check.PurchaseDate <= request.Range.End.Value.ToUniversalTime());
}

public class CheckRepository(ReceiptsContext context) : ICheckRepository
{
    public async Task<IReadOnlyList<Receipts.Core.Model.Product>> GetProductsAsync(GetChecksQuery getChecksQuery)
    {
        return (await GetChecksAsync(getChecksQuery)).SelectMany(check => check.Products).ToList();
    }

    public async Task<IReadOnlyList<Check>> GetChecksAsync(GetChecksQuery getChecksQuery)
    {
        var checksQueryable = context.Checks
            .AsNoTracking()
            .Include(c => c.Products)
            .ThenInclude(p => p.Subcategory)
            .ThenInclude(sub => sub.Category)
            .Where(getChecksQuery.GetPredicate());

        if (getChecksQuery.Take.HasValue) 
            checksQueryable = checksQueryable.Take(getChecksQuery.Take.Value);
        
        if (getChecksQuery.Skip.HasValue) 
            checksQueryable = checksQueryable.Skip(getChecksQuery.Skip.Value);
        
        var checks = await checksQueryable
                .ToListAsync();

        return checks.ConvertAll(check => check.ConvertToCheck());
    }

    public async Task<Check?> GetCheckByRequest(GetCheckRequest checkRequest)
    {
        var dbCheck = await context.Checks
            .Include(c => c.Products)
            .ThenInclude(p => p.Subcategory)
            .ThenInclude(sub => sub.Category)
            .SingleOrDefaultAsync(c =>
                c.Fn == checkRequest.Fn &&
                c.PurchaseDate == checkRequest.T &&
                c.Fd == checkRequest.Fd &&
                c.Fp == checkRequest.Fp);
        return dbCheck?.ConvertToCheck();
    }

    public async Task<Check> SaveCheck(AddCheckRequest addCheckRequest)
    {
        var categoriesNames = addCheckRequest.Products.Select(p => p.Category);
        var subcategoriesNames = addCheckRequest.Products.Select(p => p.Subcategory);
        await context.Categories
            .Where(category => categoriesNames.Any(productCategory => productCategory == category.Name))
            .LoadAsync();
        await context.Subcategories
            .Where(subcategory => subcategoriesNames.Any(productSubcategory => productSubcategory == subcategory.Name))
            .LoadAsync();
        // var userEntity = await context.Users.FindAsync(addCheckRequest.Login) ??
                         // throw new KeyNotFoundException($"User with login {addCheckRequest.Login} not found");

        var check = new Receipts.DataBase.Entities.Check
        {
            Fp = addCheckRequest.Fp,
            Fn = addCheckRequest.Fn,
            Fd = addCheckRequest.Fd,
            S = addCheckRequest.S,
            AddedDate = DateTime.UtcNow,
            PurchaseDate = addCheckRequest.PurchaseDate,
            Products = addCheckRequest.Products.Select(CreateProduct).ToList(),
            Login = addCheckRequest.Login,
        };

        context.Checks.Add(check);
        await context.SaveChangesAsync();

        return check.ConvertToCheck();
    }

    private Product CreateProduct(AddCheckRequest.Product product)
    {
        var subcategory = GetSubcategoryByName(product.Subcategory, product.Category);
        return new Product
        {
            Name = product.Name,
            Price = product.Price,
            Sum = product.Sum,
            Quantity = product.Quantity,
            Subcategory = subcategory
        };
    }

    private Category CreateCategory(string name)
    {
        var category = new Category
        {
            Name = name
        };

        context.Categories.Add(category);

        return category;
    }

    private Subcategory CreateSubcategory(string? name, Category category)
    {
        var subcategory = new Subcategory
        {
            Name = name,
            Category = category
        };

        context.Subcategories.Add(subcategory);

        return subcategory;
    }

    private Subcategory GetSubcategoryByName(string? name, string categoryName)
    {
        if (GetExistingCategory(categoryName) is { } existingCategory)
            return GetExistingSubcategory(name, existingCategory) ??
                   CreateSubcategory(name, existingCategory);

        var category = CreateCategory(categoryName);

        return CreateSubcategory(name, category);
    }

    private Subcategory? GetExistingSubcategory(string? name, Category existingCategory) =>
        context.Subcategories.Local.SingleOrDefault(
            SubcategoryByNameAndCategoryExpression(name, existingCategory.Id)) /* ??
        await context.Subcategories.SingleOrDefaultAsync(
            SubcategoryByNameAndCategoryExpression(name, existingCategory.Id))*/;

    private static Func<Subcategory, bool> SubcategoryByNameAndCategoryExpression(string? name,
        int categoryId) =>
        subcategory => subcategory.Name == name && subcategory.CategoryId == categoryId;

    private Category? GetExistingCategory(string categoryName)
    {
        return context.Categories.Local.SingleOrDefault(c => c.Name == categoryName) /*??
               await context.Categories.SingleOrDefaultAsync(c => c.Name == categoryName)*/;
    }
}