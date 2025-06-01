using Core.Extensions;
using Core.Model.Normalized;
using Core.Model.Requests;
using Core.Services;
using FnsChecksApi.Dto;
using FnsChecksApi.Dto.Categorized;
using FnsChecksApi.Dto.Fns;
using FnsChecksApi.Requests;
using Root = FnsChecksApi.Dto.Fns.Root;

namespace FnsChecksApi;

public sealed class CheckSource(ICheckService checkService, IReceiptService receiptService) : ICheckSource
{
    public async Task<NormalizedCheck> GetCheck(CheckRequest request)
    {
        var rootReceipt = GetValidResponse(await checkService.GetAsyncByRaw(new CheckRawRequest(request.RawCheck())));
        var normalizedProducts = await receiptService.GetReceipt(CreateQuery(rootReceipt));


        var products = from normalizedProduct in normalizedProducts.Items
            join fnsProduct in rootReceipt.Data.Json.Items on normalizedProduct.InitialRequest equals fnsProduct.Name
            select new NormalizedProduct(
                normalizedProduct.InitialRequest,
                fnsProduct.Quantity,
                fnsProduct.Price,
                fnsProduct.Sum,
                normalizedProduct.Category.SecondLevelCategory,
                normalizedProduct.Category.FirstLevelCategory
            );

        return new NormalizedCheck(request.T, request.Fd, request.Fn, request.Fp, request.S, products.ToList());
    }

    private static Root GetValidResponse(Receipt? receipt)
    {
        ArgumentNullException.ThrowIfNull(receipt);

        if (receipt is BadAnswerReceipt badAnswerReceipt) throw new Exception($"{badAnswerReceipt.Data}");

        if (receipt is not Root root)
            throw new InvalidOperationException("Неправильный тип ответа ФНС");

        return root;
    }

    private static Query CreateQuery(Root root) =>
        new(
            root.Data.Json.Items.Select(i => i.Name).ToList()
        );
}