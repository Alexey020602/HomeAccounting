using Core.Mappers;
using DataBase;
using DataBase.Entities;
using FnsChecksApi;
using FnsChecksApi.Dto;
using FnsChecksApi.Dto.Categorized;
using FnsChecksApi.Dto.Fns;
using FnsChecksApi.Requests;
using Microsoft.EntityFrameworkCore;
using Category = DataBase.Entities.Category;
using Product = DataBase.Entities.Product;
using Root = FnsChecksApi.Dto.Categorized.Root;

namespace Core;

public class CheckUseCase(ICheckService checkService, IReceiptService receiptService, ApplicationContext context) : ICheckUseCase
{
    public async Task<Root> GetReceipt(CheckRawRequest checkRequest)
    {
        return await ProcessReceipt(await checkService.GetAsyncByRaw(checkRequest));
    }

    public async Task<FnsChecksApi.Dto.Categorized.Root> GetReceipt(CheckRequest checkRequest)
    {
        var fnsResponse = await checkService.GetAsyncByRaw(checkRequest);

        return await ProcessReceipt(fnsResponse);
    }
    
    public async Task<Root> GetReceipt(FileInfo file)
    {
        var fnsResponse = await checkService.GetAsyncByFile(file);
        return await  ProcessReceipt(fnsResponse);
    }
    public async Task<Root> GetReceipt(Stream file)
    {
        var fnsResponse = await checkService.GetAsyncByFile(file);
        return await  ProcessReceipt(fnsResponse);
    }

    public async Task SaveCheck(CheckRequest checkRequest)
    {
        var raw = checkRequest.RawRequest();
        if (await context.Checks.SingleOrDefaultAsync(c => c.CheckRaw == raw.QrRaw) is not null)
            return;
        await SaveCheck(await checkService.GetAsyncByRaw(checkRequest), raw.QrRaw);
    }
    public async Task<Core.Model.Check> SaveCheck(CheckRawRequest checkRequest)
    {
        return (await context.Checks.SingleOrDefaultAsync(c => c.CheckRaw == checkRequest.QrRaw))?.ConvertToCheck() ??
        await SaveCheck(await checkService.GetAsyncByRaw(checkRequest), checkRequest.QrRaw);
    }
    public async Task<Core.Model.Check> SaveCheck(Receipt fnsResponse, string qrRaw)
    {
        var root = Validate(fnsResponse);

        var normalizedProsucts = await receiptService.GetReceipt(CreateQuery(root)) ?? throw new InvalidOperationException();
        
        //Берем продукты, конвертируем в продукты из БД
        //Для каждого продукта по InitialRequest находим категорию и подкатегорию:
        //Ищем подкатегорию по названию, если такой нет, ищем категорию и добавляем подкатегорию с найденной категорией, или с новой категорией
        var productsTasks = root.Data.Json.Items.Select(async (item) =>
        {
            var category = normalizedProsucts.Items.First(i => i.InitialRequest == item.Name).Category;
            var subcategory = await GetSubcategoryByName(category.SecondLevelCategory, category.FirstLevelCategory);
            return new DataBase.Entities.Product
            {
                Name = item.Name,
                Price = item.Price,
                Quantity = item.Quantity,
                Subcategory = subcategory,
            };
        });
        var products = new List<Product>(); 

        foreach (var task in productsTasks)
        {
            products.Add(await task);
        }

        var check = new Check
        {
            CheckRaw = qrRaw,
            Products = products,
        };
        
        await context.Checks.AddAsync(check);
        await context.SaveChangesAsync();
        return check.ConvertToCheck();
    }

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

    private async Task<DataBase.Entities.Subcategory> GetSubcategoryByName(string? name, string categoryName)
    {
        var existingCategory = await context.Categories.SingleOrDefaultAsync(c => c.Name == categoryName) ??
                               context.Categories.Local.SingleOrDefault(c => c.Name == categoryName);
        if (existingCategory is not null)
        {
            return await context.Subcategories.SingleOrDefaultAsync(c =>
                    c.Name == name && c.CategoryId == existingCategory.Id) ??
                   context.Subcategories.Local.SingleOrDefault(c => c.Name == name && c.CategoryId == existingCategory.Id) ??
                   await CreateSubcategory(name, existingCategory);
        }

        var category = await CreateCategory(categoryName);
        
        return await CreateSubcategory(name, category);
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

