using Checks.Core;
using Checks.DataBase.Entities;
using Checks.DataBase.Mappers;
using Microsoft.EntityFrameworkCore;
using CheckDto = Checks.Contracts.CheckDto;

namespace Checks.DataBase;

public class CheckRepository(ApplicationContext context) : ICheckRepository
{
    public async Task<IReadOnlyList<CheckDto>> GetChecksAsync(int skip = 0, int take = 100)
    {
        var checks = 
            
            await context.Checks
                .AsNoTracking()
             .Include(c => c.Products)
             .ThenInclude(p => p.Subcategory)
             .ThenInclude(sub => sub.Category)
             .Skip(skip)
             .Take(take)
             .ToListAsync();


        return checks.ConvertAll(check => check.ConvertToCheckList());
    }
    public async Task<CheckDto?> GetCheckByRequest(GetCheckRequest checkRequest)
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
    public async Task<CheckDto> SaveCheck(AddCheckRequest addCheckRequest)
    {
        await context.Categories
            .Include(category => addCheckRequest.Products.Any(product => product.Category == category.Name))
            .LoadAsync();
        await context.Subcategories
            .Include(subcategory => addCheckRequest.Products.Any(product => product.Subcategory == subcategory.Name))
            .LoadAsync();
        var userEntity = await context.Users.FindAsync(addCheckRequest.Login) ??
                         throw new KeyNotFoundException($"User with login {addCheckRequest.Login} not found");
        
        var check = new DataBase.Entities.Check
        {
            Fp = addCheckRequest.Fp,
            Fn = addCheckRequest.Fn,
            Fd = addCheckRequest.Fd,
            S = addCheckRequest.S,
            AddedDate = DateTime.UtcNow,
            PurchaseDate = addCheckRequest.PurchaseDate,
            Products = addCheckRequest.Products.Select(CreateProduct).ToList(),
            User = userEntity,
        };

        context.Checks.Add(check);
        await context.SaveChangesAsync();

        return check.ConvertToCheckList();
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
            SubcategoryByNameAndCategoryExpression(name, existingCategory.Id))/* ??
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