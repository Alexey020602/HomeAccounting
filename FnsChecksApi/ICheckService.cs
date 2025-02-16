using FnsChecksApi.Dto.Fns;
using FnsChecksApi.Requests;
using Refit;

namespace FnsChecksApi;

public interface ICheckService
{
    [Post("/api/v1/check/get")]
    public Task<Receipt> GetAsyncByRaw([Body(BodySerializationMethod.UrlEncoded)] CheckRawRequest checkRequest);
}