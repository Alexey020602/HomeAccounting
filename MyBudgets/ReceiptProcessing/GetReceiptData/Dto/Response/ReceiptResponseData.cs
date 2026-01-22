namespace MyBudgets.ReceiptProcessing.GetReceiptData.Dto.Response;

public record ReceiptResponseData(
    Receipt Json,
    string Html
);