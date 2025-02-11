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

public partial class CheckUseCase(
    ICheckService checkService,
    IReceiptService receiptService,
    ApplicationContext context,
    ILogger<CheckUseCase> logger) : ICheckUseCase
{
    public async Task<IReadOnlyList<Model.ChecksList.Check>> GetChecksAsync(int skip = 0, int take = 100)
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
    public async Task<Model.ChecksList.Check> SaveCheck(CheckRequest checkRequest)
    {
        return (await GetCheckByRequest(checkRequest))?.ConvertToCheckList() ??
               await SaveCheck(await checkService.GetAsyncByRaw(new CheckRawRequest(checkRequest.RawCheck())),
                   checkRequest);
    }

    private async Task<Model.ChecksList.Check> SaveCheck(Receipt fnsResponse, CheckRequest checkRequest)
    {
        var root = Validate(fnsResponse);

        var normalizedProducts =
            await receiptService.GetReceipt(CreateQuery(root)) ?? throw new InvalidOperationException();

        var converter = new Converter(normalizedProducts, root, context, logger);

        return await converter.ConvertToCheck(checkRequest);
    }

    private Task<Check?> GetCheckByRequest(CheckRequest checkRequest)
    {
        return context.Checks
            .Include(c => c.Products)
            .ThenInclude(p => p.Subcategory)
            .ThenInclude(sub => sub.Category)
            .SingleOrDefaultAsync(c =>
                c.Fn == checkRequest.Fn &&
                c.PurchaseDate == checkRequest.T &&
                c.Fd == checkRequest.Fd &&
                c.Fp == checkRequest.Fp);
    }

    private FnsChecksApi.Dto.Fns.Root Validate(Receipt fnsResponse)
    {
        ArgumentNullException.ThrowIfNull(fnsResponse);

        if (fnsResponse is BadAnswerReceipt badAnswerReceipt) throw new Exception($"{badAnswerReceipt.Data}");

        if (fnsResponse is not FnsChecksApi.Dto.Fns.Root root)
            throw new InvalidOperationException("Неправильный тип ответа ФНС");

        return root;
    }
    private async Task<Root> ProcessReceipt(Receipt fnsResponse)
    {
        if (fnsResponse is null) throw new NullReferenceException();

        if (fnsResponse is BadAnswerReceipt badAnswerReceipt) throw new Exception($"{badAnswerReceipt.Data}");

        if (fnsResponse is not FnsChecksApi.Dto.Fns.Root root)
            throw new InvalidOperationException("Неправильный тип ответа ФНС");

        return await receiptService
            .GetReceipt(
                CreateQuery(root)
            );
    }

    private static Query CreateQuery(FnsChecksApi.Dto.Fns.Root root)
    {
        return new Query(
            root.Data.Json.Items.Select(i => i.Name).ToList()
        );
    }
}