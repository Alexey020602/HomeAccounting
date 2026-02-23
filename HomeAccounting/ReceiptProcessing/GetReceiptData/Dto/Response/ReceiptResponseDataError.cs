namespace HomeAccounting.ReceiptProcessing.GetReceiptData.Dto.Response;

public abstract record ReceiptResponseDataError(string Data) : ReceiptResponse();

public record IncorrectReceiptResponseError(string Data) : ReceiptResponseDataError(Data);

public record ReceiptResponseDataErrorDataNotReceivedYetError(string Data)
    : ReceiptResponseDataError(Data);

public record NumberOfRequestsExceededError(string Data) : ReceiptResponseDataError(Data);

public record WaitingBeforeRepeatRequestError(string Data) : ReceiptResponseDataError(Data);

public record OtherReceiptResponseError(string Data) : ReceiptResponseDataError(Data);