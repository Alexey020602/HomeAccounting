using System.Linq.Expressions;
using Core.Extensions;
using Core.Mappers;
using Core.Services;
using Core.Model;
using DataBase;
using DataBase.Entities;
using FnsChecksApi;
using FnsChecksApi.Dto;
using FnsChecksApi.Dto.Categorized;
using FnsChecksApi.Dto.Fns;
using FnsChecksApi.Requests;
using Microsoft.EntityFrameworkCore;
using Category = DataBase.Entities.Category;
using Check = DataBase.Entities.Check;
using Subcategory = DataBase.Entities.Subcategory;
using CheckRequest = Core.Model.CheckRequest;
using Item = FnsChecksApi.Dto.Fns.Item;
using Product = DataBase.Entities.Product;
using Root = FnsChecksApi.Dto.Categorized.Root;

namespace Core;

public partial class CheckUseCase(ICheckService checkService, IReceiptService receiptService, ApplicationContext context) : ICheckUseCase
{
    private sealed class Converter(Root normalizedProducts, FnsChecksApi.Dto.Fns.Root root, ApplicationContext context)
    {

        public async Task<Core.Model.Check> ConvertToCheck(CheckRequest checkRequest)
        {
            var productTasks = root.Data.Json.Items.Select(CreateProduct);
            var products = new List<Product>();
            foreach (var productTask in productTasks)
            {
                products.Add(await productTask);
            }
            
            var check = new Check
            {
                Fp = checkRequest.Fp,
                Fn = checkRequest.Fn,
                Fd = checkRequest.Fd,
                S = checkRequest.S,
                AddedDate = DateTime.Now,
                PurchaseDate = checkRequest.T,
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
    // public async Task SaveCheck(CheckRequest checkRequest)
    // {
    //     var raw = checkRequest.RawRequest();
    //     if (await context.Checks.SingleOrDefaultAsync(c => c.CheckRaw == raw.QrRaw) is not null)
    //         return;
    //     await SaveCheck(await checkService.GetAsyncByRaw(checkRequest), raw.QrRaw);
    // }
    public async Task<Core.Model.Check> SaveCheck(string qrRaw)
    {
        return (await GetCheckByRequest(new CheckRequest(qrRaw)))?.ConvertToCheck() ??
        await SaveCheck(await checkService.GetAsyncByRaw(new CheckRawRequest(qrRaw)), qrRaw);
    }

    private async Task<Core.Model.Check> SaveCheck(Receipt fnsResponse, string qrRaw)
    {
        var root = Validate(fnsResponse);

        var normalizedProducts = await receiptService.GetReceipt(CreateQuery(root)) ?? throw new InvalidOperationException();
        
        //Берем продукты, конвертируем в продукты из БД
        //Для каждого продукта по InitialRequest находим категорию и подкатегорию:
        //Ищем подкатегорию по названию, если такой нет, ищем категорию и добавляем подкатегорию с найденной категорией, или с новой категорией
        // var productsTasks = root.Data.Json.Items.Select(async (item) =>
        // {
        //     var category = normalizedProducts.Items.First(i => i.InitialRequest == item.Name).Category;
        //     var subcategory = await GetSubcategoryByName(category.SecondLevelCategory, category.FirstLevelCategory);
        //     return new Product
        //     {
        //         Name = item.Name,
        //         Price = item.Price,
        //         Quantity = item.Quantity,
        //         Subcategory = subcategory,
        //     };
        // });
        // var products = new List<Product>(); 
        //
        // foreach (var task in productsTasks)
        // {
        //     products.Add(await task);
        // }
        //
        // var check = new Check
        // {
        //     CheckRaw = qrRaw,
        //     Products = products,
        // };
        //
        // await context.Checks.AddAsync(check);
        // await context.SaveChangesAsync();
        // return check.ConvertToCheck();
        return await new Converter(normalizedProducts, root, context).ConvertToCheck(new CheckRequest(qrRaw));
    }
    
    // private Task<Check?> GetCheckByRaw(string qrraw) => context.Checks
    //     .Include(c => c.Products)
    //     .ThenInclude(p => p.Subcategory)
    //     .ThenInclude(sub => sub.Category)
    //     .SingleOrDefaultAsync(c => c.CheckRaw == qrraw);
    private Task<Check?> GetCheckByRequest(CheckRequest checkRequest) => context.Checks
        .Include(c => c.Products)
        .ThenInclude(p => p.Subcategory)
        .ThenInclude(sub => sub.Category)
        .SingleOrDefaultAsync(c => 
            c.Fn == checkRequest.Fn && 
            c.PurchaseDate == checkRequest.T &&
            c.Fd == checkRequest.Fd &&
            c.Fp == checkRequest.Fp);
    private FnsChecksApi.Dto.Fns.Root Validate(Receipt fnsResponse)
    {
        ArgumentNullException.ThrowIfNull(fnsResponse);
        
        if (fnsResponse is BadAnswerReceipt badAnswerReceipt)
        {
            throw new Exception($"{badAnswerReceipt.Data}");
        }

        if (fnsResponse is not FnsChecksApi.Dto.Fns.Root root)
        {
            throw new InvalidOperationException("Неправильный тип ответа ФНС");
        }
        
        return root;
    }
    

    private async Task<Root> ProcessReceipt(Receipt fnsResponse)
    {
        if (fnsResponse is null)
        {
            throw new NullReferenceException();
        }
        
        if (fnsResponse is BadAnswerReceipt badAnswerReceipt)
        {
            throw new Exception($"{badAnswerReceipt.Data}");
        }

        if (fnsResponse is not FnsChecksApi.Dto.Fns.Root root)
        {
            throw new InvalidOperationException("Неправильный тип ответа ФНС");
        }
        
        return await receiptService
            .GetReceipt(
                CreateQuery(root)
            );
    }

    private static Query CreateQuery(FnsChecksApi.Dto.Fns.Root root) => new(
        root.Data.Json.Items.Select(i => i.Name).ToList()
    );
}

