using System.Linq.Expressions;
using Core.Mappers;
using Core.Model;
using Core.Model.Normalized;
using Core.Model.Requests;
using Core.Services;
using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Check = Core.Model.ChecksList.Check;

namespace DataBase;

public class CheckRepository(ApplicationContext context) : ICheckRepository
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


        return checks.ConvertAll(check => check.ConvertToCheckList());
    }
    public async Task<Check?> GetCheckByRequest(CheckRequest checkRequest)
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
        return dbCheck?.ConvertToCheckList();
    }
    public async Task<Check> SaveCheck(NormalizedCheck normalizedCheck)
    {
        var products = new List<Product>(normalizedCheck.Products.Count);
        foreach (var product in normalizedCheck.Products)
        {
            products.Add(await CreateProduct(product));
        }

        var check = new DataBase.Entities.Check
        {
            Fp = normalizedCheck.Fp,
            Fn = normalizedCheck.Fn,
            Fd = normalizedCheck.Fd,
            S = normalizedCheck.Sum,
            AddedDate = DateTime.UtcNow,
            PurchaseDate = normalizedCheck.PurchaseDate,
            Products = products
        };

        await context.Checks.AddAsync(check);
        await context.SaveChangesAsync();

        return check.ConvertToCheckList();
    }
    
    private async Task<Product> CreateProduct(NormalizedProduct product)
    {
        var subcategory = await GetSubcategoryByName(product.Subcategory, product.Category);
        return new Product
        {
            Name = product.Name,
            Price = product.Price,
            Sum = product.Sum,
            Quantity = product.Quantity,
            Subcategory = subcategory
        };
    }
    
    private async Task<Category> CreateCategory(string name)
    {
        var category = new Category
        {
            Name = name
        };
        var entry = context.Categories.Attach(category);
        await context.Categories.AddAsync(category);

        return category;
    }
    
    private async Task<Subcategory> CreateSubcategory(string? name, Category category)
    {
        var subcategory = new Subcategory
        {
            Name = name,
            Category = category
        };

        context.Subcategories.Attach(subcategory);
        await context.Subcategories.AddAsync(subcategory);

        return subcategory;
    }

    private async Task<Subcategory> GetSubcategoryByName(string? name, string categoryName)
    {
        var existingCategory = await GetExistingCategory(categoryName);
        if (existingCategory is not null)
            return await GetExistingSubcategory(name, existingCategory) ??
                   await CreateSubcategory(name, existingCategory);

        var category = await CreateCategory(categoryName);

        return await CreateSubcategory(name, category);
    }
    
    private async Task<Subcategory?> GetExistingSubcategory(string? name, Category existingCategory) =>
        context.Subcategories.Local.SingleOrDefault(
            SubcategoryByNameAndCategoryExpression(name, existingCategory.Id).Compile()) ??
        await context.Subcategories.SingleOrDefaultAsync(
            SubcategoryByNameAndCategoryExpression(name, existingCategory.Id));

    private static Expression<Func<Subcategory, bool>> SubcategoryByNameAndCategoryExpression(string? name,
        int categoryId) =>
        subcategory => subcategory.Name == name && subcategory.CategoryId == categoryId;

    private async Task<Category?> GetExistingCategory(string categoryName)
    {
        return context.Categories.Local.SingleOrDefault(c => c.Name == categoryName) ??
               await context.Categories.SingleOrDefaultAsync(c => c.Name == categoryName);
    }
}