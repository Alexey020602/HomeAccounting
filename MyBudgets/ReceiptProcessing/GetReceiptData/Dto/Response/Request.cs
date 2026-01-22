namespace MyBudgets.ReceiptProcessing.GetReceiptData.Dto.Response;

public record Request(
    string Qrurl,
    string Qrfile,
    string Qrraw,
    Manual Manual
);