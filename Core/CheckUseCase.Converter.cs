using System.Linq.Expressions;
using Core.Mappers;
using DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Category = DataBase.Entities.Category;
using Check = DataBase.Entities.Check;
using Subcategory = DataBase.Entities.Subcategory;
using CheckRequest = Core.Model.Requests.CheckRequest;
using Item = FnsChecksApi.Dto.Fns.Item;
using Product = DataBase.Entities.Product;
using Root = FnsChecksApi.Dto.Categorized.Root;

namespace Core;

public partial class CheckUseCase
{
    private sealed class Converter(Root normalizedProducts, FnsChecksApi.Dto.Fns.Root root, ApplicationContext context, ILogger<CheckUseCase> logger)
    {

        public async Task<Core.Model.Check> ConvertToCheck(CheckRequest checkRequest)
        {
            var productTasks = root.Data.Json.Items.Select(CreateProduct);
            var products = new List<Product>();
            foreach (var productTask in productTasks)
            {
                products.Add(await productTask);
            }

            var addedDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            logger.LogInformation($"AddedDate: {addedDate} Kind: {addedDate.Kind}");

            var purchaseDate = DateTime.SpecifyKind(checkRequest.T.ToUniversalTime(), DateTimeKind.Utc);
            
            logger.LogInformation($"PurchaseDate: {purchaseDate} Kind: {purchaseDate.Kind}");
            var check = new Check
            {
                Fp = checkRequest.Fp,
                Fn = checkRequest.Fn,
                Fd = checkRequest.Fd,
                S = checkRequest.S,
                AddedDate = addedDate,
                PurchaseDate = purchaseDate,
                Products = products,
            };
        
            await context.Checks.AddAsync(check);
            await context.SaveChangesAsync();
            return check.ConvertToCheck();
        }
        private async Task<Product> CreateProduct(Item item)
        {
            var category = normalizedProducts.Items.First(i => i.InitialRequest == item.Name).Category;
            
            var subcategory = await GetSubcategoryByName(category.SecondLevelCategory, category.FirstLevelCategory);
            return new Product
            {
                Name = item.Name,
                Price = item.Price,
                Quantity = item.Quantity,
                Subcategory = subcategory,
            };
        } 
        
        private async Task<Category> GetCategoryByName(string name) =>
        await context.Categories.SingleOrDefaultAsync(c => c.Name == name) ?? new Category
        {
            Name = name,
        };

    private async Task<Category> CreateCategory(string name)
    {
        var category = new Category
        {
            Name = name,
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
            Category = category,
        };
        
        context.Subcategories.Attach(subcategory);
        await context.Subcategories.AddAsync(subcategory);
        // await context.SaveChangesAsync();
        return subcategory;
    }

    private async Task<Subcategory> GetSubcategoryByName(string? name, string categoryName)
    {
        var existingCategory = await GetExistingCategory(categoryName);
        if (existingCategory is not null)
        {
            return await GetExistingSubcategory(name, existingCategory) ??
                   await CreateSubcategory(name, existingCategory);
        }

        var category = await CreateCategory(categoryName);
        
        return await CreateSubcategory(name, category);
    }

    private async Task<Subcategory?> GetExistingSubcategory(string? name, Category existingCategory) =>
        context.Subcategories.Local.SingleOrDefault(SubcategoryByNameAndCategoryExpression(name, existingCategory.Id).Compile()) ?? 
        await context.Subcategories.SingleOrDefaultAsync(SubcategoryByNameAndCategoryExpression(name, existingCategory.Id));

    private static Expression<Func<Subcategory, bool>> SubcategoryByNameAndCategoryExpression(string? name, int categoryId) => 
        subcategory => subcategory.Name == name && subcategory.CategoryId == categoryId;

    private async Task<Category?> GetExistingCategory(string categoryName) =>
        context.Categories.Local.SingleOrDefault(c => c.Name == categoryName) ??
        await context.Categories.SingleOrDefaultAsync(c => c.Name == categoryName);
    }
}