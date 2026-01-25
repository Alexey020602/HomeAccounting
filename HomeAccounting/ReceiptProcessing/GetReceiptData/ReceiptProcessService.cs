using HomeAccounting.ReceiptProcessing.Contracts;
using HomeAccounting.ReceiptProcessing.GetReceiptData.Dto.Response;

namespace HomeAccounting.ReceiptProcessing.GetReceiptData;

internal sealed class ReceiptProcessService(ICheckService checkService) : IReceiptProcessService
{
    public async Task<GetReceiptDataResponse> GetReceiptData(GetReceiptDataRequest request,
        CancellationToken cancellationToken)
    {
        var response =
            await checkService.GetAsyncByRaw(new CheckRawRequest(request.ReceiptFiscalData.Raw), cancellationToken);

        switch (response)
        {
            case SuccessfulReceiptResponseResponse succesfulResponse:
                return new GetReceiptDataResponse(succesfulResponse.Data.Json.Items
                    .Select(item => new ReceiptProduct(item.Name, item.Quantity, item.Price, item.Sum))
                    .ToList());
            case IncorrectReceiptResponseError incorrectReceiptResponseError:
                throw new ReceiptProcessException(incorrectReceiptResponseError.Data);
            case ReceiptResponseDataErrorDataNotReceivedYetError dataErrorDataNotReceivedYetError:
                throw new ReceiptProcessException(dataErrorDataNotReceivedYetError.Data);
            case NumberOfRequestsExceededError numberOfRequestsExceededError: 
                throw new ReceiptProcessException(numberOfRequestsExceededError.Data);
            case WaitingBeforeRepeatRequestError beforeRepeatRequestError:
                throw new ReceiptProcessException(beforeRepeatRequestError.Data);
            case OtherReceiptResponseError otherReceiptResponseError:
                throw new ReceiptProcessException(otherReceiptResponseError.Data);
            default: 
                throw new ReceiptProcessException("Unknown response");
        }
    }
}

public sealed class ReceiptProcessException(string? message = null, Exception? innerException = null)
    : Exception(message, innerException);