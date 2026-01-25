namespace HomeAccounting.ReceiptProcessing.Contracts;

internal sealed record GetReceiptDataResponse(IReadOnlyCollection<ReceiptProduct> Products);