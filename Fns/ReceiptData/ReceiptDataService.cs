using Fns.Contracts;
using Fns.Contracts.ReceiptData;
using Fns.ReceiptData.ProverkaCheka.Dto;
using MaybeResults;
using Receipt = Fns.Contracts.ReceiptData.Receipt;

namespace Fns.ReceiptData;

sealed class ReceiptDataService(ICheckService checkService): IReceiptDataService
{
    public async Task<IMaybe<Receipt>> GetReceipt(ReceiptFiscalData receiptFiscalData)
    {
        var fnsReceipt = await checkService.GetAsyncByRaw(new(receiptFiscalData.RawCheck()));
        return from receipt in GetValidResponse(fnsReceipt)
            select ConvertReceiptResponseToReceipt(receipt, receiptFiscalData);
    }

    private static Receipt ConvertReceiptResponseToReceipt(
        SuccessfulReceiptResponseResponse receiptResponse,
        ReceiptFiscalData receiptFiscalData) => new Receipt(
        receiptFiscalData,
        ConvertReceiptItemsToProducts(receiptResponse)
    );

    private static List<ReceiptProduct> ConvertReceiptItemsToProducts(
        SuccessfulReceiptResponseResponse receiptResponse) => receiptResponse
        .Data.Json.Items
        .Select(ConvertReceiptItemToProduct)
        .ToList();
    private static ReceiptProduct ConvertReceiptItemToProduct(ReceiptItem item) => new ReceiptProduct(item.Name, item.Quantity, item.Price, item.Sum);
    private static IMaybe<SuccessfulReceiptResponseResponse> GetValidResponse(ReceiptData.ProverkaCheka.Dto.ReceiptResponse? receipt)
    {
        ArgumentNullException.ThrowIfNull(receipt);

        if (receipt is ReceiptResponseDataError badAnswerReceipt) throw new FnsException($"{badAnswerReceipt.Data}");

        if (receipt is not ProverkaCheka.Dto.SuccessfulReceiptResponseResponse) throw new FnsException("Invalid response type from FNS");

        if (receipt is not ProverkaCheka.Dto.SuccessfulReceiptResponseResponse root)
            throw new InvalidOperationException("Неправильный тип ответа ФНС");

        return root;
    }
    
    private static IMaybe<SuccessfulReceiptResponseResponse> 
}