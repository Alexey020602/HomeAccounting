namespace Fns.ReceiptData.ProverkaCheka.Dto;

public abstract record ReceiptResponseDataError(int Code, string Data) : ReceiptResponse(Code);
public record IncorrectReceiptResponseError(int Code, string Data) : ReceiptResponseDataError(Code, Data);
public record ReceiptResponseDataErrorDataNotReceivedYetError(int Code, string Data) : ReceiptResponseDataError(Code, Data);
public record NumberOfRequestsExceededError(int Code, string Data) : ReceiptResponseDataError(Code, Data);
public record WaitingBeforeRepeatRequestError(int Code, string Data) : ReceiptResponseDataError(Code, Data);
public record OtherReceiptResponseError(int Code, string Data) : ReceiptResponseDataError(Code, Data);