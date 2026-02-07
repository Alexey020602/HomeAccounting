namespace HomeAccounting.ReceiptProcessing.GetReceiptData.Dto.Response;

public record SuccessfulReceiptResponseResponse(
    int First,
    ReceiptResponseData Data
    // Request Request
) : ReceiptResponse();