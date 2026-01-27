namespace HomeAccounting.ReceiptProcessing.Contracts;

internal sealed record GetReceiptDataResponse(string PurchasePlase, IReadOnlyCollection<ReceiptProduct> Products);