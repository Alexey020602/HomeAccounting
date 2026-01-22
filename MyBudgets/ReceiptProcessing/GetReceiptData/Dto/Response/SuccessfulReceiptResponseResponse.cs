namespace MyBudgets.ReceiptProcessing.GetReceiptData.Dto.Response;

public record SuccessfulReceiptResponseResponse(
    int Code,
    int First,
    ReceiptResponseData Data
    // Request Request
) : ReceiptResponse(Code);