using DataBase;
using DataBase.Entities;
using FnsChecksApi.Dto;
using FnsChecksApi.Dto.Categorized;
using FnsChecksApi.Dto.Fns;
using FnsChecksApi.Requests;
using Microsoft.EntityFrameworkCore;
using Category = DataBase.Entities.Category;
using Product = DataBase.Entities.Product;
using Root = FnsChecksApi.Dto.Categorized.Root;

namespace FnsChecksApi;

public class CheckUseCase(ICheckService checkService, IReceiptService receiptService, ApplicationContext context) : ICheckUseCase
{
    public async Task<Root> GetReceipt(CheckRawRequest checkRequest)
    {
        return await ProcessReceipt(await checkService.GetAsyncByRaw(checkRequest));
    }

    public async Task<Dto.Categorized.Root> GetReceipt(CheckRequest checkRequest)
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

    private async Task SaveCheck(Receipt fnsResponse)
    {
        ArgumentNullException.ThrowIfNull(fnsResponse);
        
        if (fnsResponse is BadAnswerReceipt badAnswerReceipt)
        {
            throw new Exception($"{badAnswerReceipt.Data}");
        }

        if (fnsResponse is not Dto.Fns.Root root)
        {
            throw new InvalidOperationException("Неправильный тип ответа ФНС");
        }

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
        var products = await Task.WhenAll(productsTasks);
        
        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }

    private async Task<Category> GetCategoryByName(string name) =>
        await context.Categories.SingleOrDefaultAsync(c => c.Name == name) ?? new Category
        {
            Name = name,
        };

    private async Task<DataBase.Entities.Subcategory> GetSubcategoryByName(string? name, string categoryName)
    {
        var existingCategory = await context.Categories.SingleOrDefaultAsync(c => c.Name == categoryName);
        if (existingCategory is not null)
        {
            return await context.Subcategories.SingleOrDefaultAsync(c =>
                    c.Name == name && c.CategoryId == existingCategory.Id) ?? new Subcategory
                {
                    Name = name,
                    CategoryId = existingCategory.Id,
                };
        }

        var category = new Category
        {
            Name = categoryName,
        };
        var subcategory = new Subcategory
        {
            Name = name,
            Category = category,
        };
        
        return subcategory;
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

        if (fnsResponse is not Dto.Fns.Root root)
        {
            throw new InvalidOperationException("Неправильный тип ответа ФНС");
        }
        
        return await receiptService
            .GetReceipt(
                CreateQuery(root)
            );
    }

    private static Query CreateQuery(Dto.Fns.Root root) => new(
        root.Data.Json.Items.Select(i => i.Name).ToList()
    );
}

