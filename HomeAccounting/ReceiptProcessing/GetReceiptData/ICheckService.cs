using HomeAccounting.ReceiptProcessing.GetReceiptData.Dto.Response;
using Refit;

namespace HomeAccounting.ReceiptProcessing.GetReceiptData;

public interface ICheckService
{
    [Post("/api/v1/check/get")]
    public Task<ReceiptResponse> GetAsyncByRaw([Body(BodySerializationMethod.UrlEncoded)] CheckRawRequest checkRequest,
        CancellationToken cancellationToken);
}