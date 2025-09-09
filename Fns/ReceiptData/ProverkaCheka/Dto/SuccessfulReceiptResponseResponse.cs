namespace Fns.ReceiptData.ProverkaCheka.Dto;

public record SuccessfulReceiptResponseResponse(
    int Code,
    int First,
    ReceiptResponseData Data
    // Request Request
) : ReceiptResponse(Code);