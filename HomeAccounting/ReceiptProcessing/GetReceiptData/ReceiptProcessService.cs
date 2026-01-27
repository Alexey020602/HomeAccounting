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
                return new GetReceiptDataResponse(
                    succesfulResponse.Data.Json.User,
                    succesfulResponse.Data.Json.Items
                        .Select(item => new ReceiptProduct(item.Name, item.Quantity, item.Price, item.Sum))
                        .ToList());

            case IncorrectReceiptResponseError incorrect:
                throw new IncorrectReceiptProcessException(incorrect.Data);

            case ReceiptResponseDataErrorDataNotReceivedYetError notReceived:
                throw new DataNotReceivedYetProcessException(notReceived.Data);

            case NumberOfRequestsExceededError exceeded:
                throw new NumberOfRequestsExceededProcessException(exceeded.Data);

            case WaitingBeforeRepeatRequestError waiting:
                throw new WaitingBeforeRepeatRequestProcessException(waiting.Data);

            case OtherReceiptResponseError other:
                throw new OtherReceiptProcessException(other.Data);

            default:
                throw new UnknownReceiptProcessException("Unknown response");
        }
    }
}