using Fns.ProverkaCheka.Dto;
using Fns.Requests;
using Refit;

namespace Fns;

public interface ICheckService
{
    [Post("/api/v1/check/get")]
    public Task<Receipt> GetAsyncByRaw([Body(BodySerializationMethod.UrlEncoded)] CheckRawRequest checkRequest);
}