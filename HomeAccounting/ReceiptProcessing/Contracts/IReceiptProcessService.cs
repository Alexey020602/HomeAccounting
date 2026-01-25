namespace HomeAccounting.ReceiptProcessing.Contracts;

internal interface IReceiptProcessService
{
    Task<GetReceiptDataResponse> GetReceiptData(GetReceiptDataRequest request, CancellationToken cancellationToken);
}