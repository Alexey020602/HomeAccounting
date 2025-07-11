using Fns.Contracts;
using Fns.Contracts.ReceiptData;
using Fns.ReceiptData.ProverkaCheka.Dto;
using Receipt = Fns.Contracts.ReceiptData.Receipt;

namespace Fns.ReceiptData;

sealed class ReceiptDataService(ICheckService checkService): IReceiptDataService
{
    public async Task<Receipt> GetReceipt(ReceiptFiscalData receiptFiscalData)
    {
        var fnsReceipt = await checkService.GetAsyncByRaw(new(receiptFiscalData.RawCheck()));
        var receipt = GetValidResponse(fnsReceipt);
        return new Receipt(receiptFiscalData, receipt.Data.Json.Items.Select(item => new ReceiptProduct(item.Name, item.Quantity, item.Price, item.Sum)).ToList());
    }
    
    private static Root GetValidResponse(ReceiptData.ProverkaCheka.Dto.Receipt? receipt)
    {
        ArgumentNullException.ThrowIfNull(receipt);

        FnsException.ThrowIfInvalidResponse(receipt);

        if (receipt is not Root root)
            throw new InvalidOperationException("Неправильный тип ответа ФНС");

        return root;
    }
}