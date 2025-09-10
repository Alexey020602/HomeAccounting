using Fns.Contracts;
using Fns.Contracts.ReceiptData;
using Fns.ReceiptData.ProverkaCheka.Dto;
using MaybeResults;
using Shared.Utils;
using Receipt = Fns.Contracts.ReceiptData.Receipt;

namespace Fns.ReceiptData;

sealed class ReceiptDataService(ICheckService checkService) : IReceiptDataService
{
    public async Task<IMaybe<Receipt>> GetReceipt(ReceiptFiscalData receiptFiscalData)
    {
        var fnsReceipt = await checkService.GetAsyncByRaw(new(receiptFiscalData.RawCheck()));
        return from receipt in GetValidResponse(fnsReceipt)
            select ConvertReceiptResponseToReceipt(receipt, receiptFiscalData);
    }

    private static Receipt ConvertReceiptResponseToReceipt(
        SuccessfulReceiptResponseResponse receiptResponse,
        ReceiptFiscalData receiptFiscalData) => new Receipt(
        receiptFiscalData,
        ConvertReceiptItemsToProducts(receiptResponse)
    );

    private static List<ReceiptProduct> ConvertReceiptItemsToProducts(
        SuccessfulReceiptResponseResponse receiptResponse) => receiptResponse
        .Data.Json.Items
        .Select(ConvertReceiptItemToProduct)
        .ToList();

    private static ReceiptProduct ConvertReceiptItemToProduct(ReceiptItem item) =>
        new ReceiptProduct(item.Name, item.Quantity, item.Price, item.Sum);

    private static IMaybe<SuccessfulReceiptResponseResponse> GetValidResponse(
        ReceiptResponse? receipt)
    {
        ArgumentNullException.ThrowIfNull(receipt);

        return receipt switch
        {
            SuccessfulReceiptResponseResponse response => Maybe.Create(response),
            IncorrectReceiptResponseError incorrectReceiptResponseError => IncorrectReceiptError
                .CreateFromReceiptError(incorrectReceiptResponseError)
                .Cast<SuccessfulReceiptResponseResponse>(),
            ReceiptResponseDataErrorDataNotReceivedYetError receiptResponseDataErrorDataNotReceivedYetError =>
                ReceiptDataNotReceivedByFnsError<SuccessfulReceiptResponseResponse>.Create(
                    receiptResponseDataErrorDataNotReceivedYetError.Data),
            ProverkaCheka.Dto.NumberOfRequestsExceededError numberOfRequestsExceededError =>
                NumberOfRequestsExceededError<SuccessfulReceiptResponseResponse>.Create(numberOfRequestsExceededError
                    .Data),
            ProverkaCheka.Dto.WaitingBeforeRepeatRequestError waitingBeforeRepeatRequestError =>
                WaitingBeforeRepeatRequestError<SuccessfulReceiptResponseResponse>.Create(
                    waitingBeforeRepeatRequestError.Data),
            OtherReceiptResponseError receiptResponseError => UnknownReceiptError<SuccessfulReceiptResponseResponse>
                .Create(receiptResponseError.Data),
            _ => throw new FnsException("Unknown response")
        };
    }
}

[None]
sealed partial record IncorrectReceiptError
{
    public static IncorrectReceiptError CreateFromReceiptError(
        IncorrectReceiptResponseError incorrectReceiptResponseError)
        => new IncorrectReceiptError(incorrectReceiptResponseError.Data);
}

[None]
sealed partial record ReceiptDataNotReceivedByFnsError;

[None]
public partial record NumberOfRequestsExceededError : IManyRequestsError;

[None]
public partial record WaitingBeforeRepeatRequestError : IManyRequestsError;

[None]
public partial record UnknownReceiptError;