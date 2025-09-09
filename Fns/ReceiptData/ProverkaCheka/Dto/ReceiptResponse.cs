using System.Text.Json.Serialization;

namespace Fns.ReceiptData.ProverkaCheka.Dto;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "code")]
[JsonDerivedType(typeof(IncorrectReceiptResponseError), 0)]
[JsonDerivedType(typeof(SuccessfulReceiptResponseResponse), 1)]
[JsonDerivedType(typeof(ReceiptResponseDataErrorDataNotReceivedYetError), 2)]
[JsonDerivedType(typeof(NumberOfRequestsExceededError), 3)]
[JsonDerivedType(typeof(WaitingBeforeRepeatRequestError), 4)]
[JsonDerivedType(typeof(OtherReceiptResponseError), 5)]
public abstract record ReceiptResponse(int Code);