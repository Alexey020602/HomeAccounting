namespace MyBudgets.ReceiptProcessing.Contracts;

internal sealed record GetReceiptDataResponse(IReadOnlyCollection<ReceiptProduct> Products);