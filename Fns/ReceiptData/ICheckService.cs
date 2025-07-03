using Fns.ReceiptData.ProverkaCheka.Dto;
using Refit;

namespace Fns.ReceiptData;

public interface ICheckService
{
    [Post("/api/v1/check/get")]
    public Task<Receipt> GetAsyncByRaw([Body(BodySerializationMethod.UrlEncoded)] CheckRawRequest checkRequest);
}