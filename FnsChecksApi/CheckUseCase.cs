using FnsChecksApi.Dto;
using FnsChecksApi.Dto.Categorized;
using FnsChecksApi.Requests;
using Microsoft.AspNetCore.Http;
using Root = FnsChecksApi.Dto.Categorized.Root;

namespace FnsChecksApi;

public class CheckUseCase(ICheckService checkService, IReceiptService receiptService) : ICheckUseCase
{
    public async Task<Dto.Categorized.Root> GetReceipt(CheckRequest checkRequest)
    {
        var fnsResponse = await checkService.GetAsyncByRaw(checkRequest);

        return await ProcessReceipt(fnsResponse);
    }

    public async Task<Root> GetReceipt(IFormFile file)
    {
        var fnsResponse = await checkService.GetAsyncByFormFile(file);
        return await  ProcessReceipt(fnsResponse);
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

    private async Task<Root> ProcessReceipt(Receipt fnsResponse)
    {
        if (fnsResponse is null)
        {
            throw new NullReferenceException();
        }
        
        if (fnsResponse is BadAnswerReceipt badAnswerReceipt)
        {
            throw new Exception($"Превышено количество запросов:\n{badAnswerReceipt.Data}");
        }

        if (fnsResponse is not Dto.Root root)
        {
            throw new InvalidOperationException("Неправильный тип ответа ФНС");
        }
        
        return await receiptService
            .GetReceipt(
                CreateQuery(root)
            );
    }

    private static Query CreateQuery(Dto.Root root) => new(
        root.Data.Json.Items.Select(i => i.Name).ToList()
    );
}

