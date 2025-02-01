using Core.Extensions;
using Core.Mappers;
using Core.Services;
using DataBase;
using FnsChecksApi;
using FnsChecksApi.Dto;
using FnsChecksApi.Dto.Categorized;
using FnsChecksApi.Dto.Fns;
using FnsChecksApi.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Check = DataBase.Entities.Check;
using CheckRequest = Core.Model.Requests.CheckRequest;
using Root = FnsChecksApi.Dto.Categorized.Root;

namespace Core;

public partial class CheckUseCase(ICheckService checkService, IReceiptService receiptService, ApplicationContext context, ILogger<CheckUseCase> logger) : ICheckUseCase
{
    
    // public async Task SaveCheck(CheckRequest checkRequest)
    // {
    //     var raw = checkRequest.RawRequest();
    //     if (await context.Checks.SingleOrDefaultAsync(c => c.CheckRaw == raw.QrRaw) is not null)
    //         return;
    //     await SaveCheck(await checkService.GetAsyncByRaw(checkRequest), raw.QrRaw);
    // }
    public async Task<Core.Model.Check> SaveCheck(CheckRequest checkRequest)
    {
        return (await GetCheckByRequest(checkRequest))?.ConvertToCheck() ??
        await SaveCheck(await checkService.GetAsyncByRaw(new CheckRawRequest(checkRequest.RawCheck())), checkRequest);
    }

    private async Task<Core.Model.Check> SaveCheck(Receipt fnsResponse, CheckRequest checkRequest)
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

        var converter = new Converter(normalizedProducts, root, context, logger);

        var check = await converter.ConvertToCheck(checkRequest);
        
        logger.LogInformation($"PurchaseDate: {check.PurchaseDate} Kind: {check.PurchaseDate.Kind}");
        logger.LogInformation($"PurchaseDate: {check.AddedDate} Kind: {check.AddedDate.Kind}");
        return check;
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

