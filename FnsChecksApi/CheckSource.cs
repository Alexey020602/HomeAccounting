using Core.Extensions;
using Core.Model;
using Core.Model.Normalized;
using Core.Model.Requests;
using FnsChecksApi;
using FnsChecksApi.Dto;
using FnsChecksApi.Dto.Categorized;
using FnsChecksApi.Dto.Fns;
using FnsChecksApi.Requests;
using Root = FnsChecksApi.Dto.Fns.Root;

namespace Core.Services;

public class CheckSource(ICheckService checkService, IReceiptService receiptService) : ICheckSource
{
    public async Task<NormalizedCheck> GetCheck(CheckRequest request)
    {
        var rootReceipt = GetValidResponse(await checkService.GetAsyncByRaw(new CheckRawRequest(request.RawCheck())));
        var normalizedProducts = await receiptService.GetReceipt(CreateQuery(rootReceipt));

        var products = normalizedProducts.Items.Select(normalizedItem =>
        {
            var product = rootReceipt.Data.Json.Items.First(item => item.Name == normalizedItem.InitialRequest);
            
            return new NormalizedProduct(
                normalizedItem.InitialRequest,
                product.Quantity,
                product.Price,
                product.Sum,
                normalizedItem.Category.SecondLevelCategory,
                normalizedItem.Category.FirstLevelCategory);
        }).ToList();

        return new NormalizedCheck(request.T, request.Fd, request.Fn, request.Fp, request.S, products);
    }

    private static Root GetValidResponse(Receipt? receipt)
    {
        ArgumentNullException.ThrowIfNull(receipt);
        
        if (receipt is BadAnswerReceipt badAnswerReceipt) throw new Exception($"{badAnswerReceipt.Data}");
        
        if(receipt is not Root root)
            throw new InvalidOperationException("Неправильный тип ответа ФНС");

        return root;
    }

    private static Query CreateQuery(Root root) =>
        new(
            root.Data.Json.Items.Select(i => i.Name).ToList()
        );
}