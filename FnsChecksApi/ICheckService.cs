using FnsChecksApi.Dto.Fns;
using FnsChecksApi.Requests;
using Refit;

namespace FnsChecksApi;

public interface ICheckService
{
    [Post("/api/v1/check/get")]
    public Task<Receipt> GetAsyncByRaw([Body(BodySerializationMethod.UrlEncoded)] CheckRawRequest checkRequest);

    [Post("/api/v1/check/get")]
    public Task<Receipt> GetAsyncByRaw( /*[Body(BodySerializationMethod.UrlEncoded)]*/ CheckRequest checkRequest);

    [Post("/api/v1/check/get")]
    [Multipart]
    public Task<Receipt> GetAsyncByFile(FileInfo qrFile, string token = "15239.20dUQQYmlHxbOPLzb");

    [Post("/api/v1/check/get")]
    [Multipart]
    public Task<Receipt> GetAsyncByFile(Stream qrFile,
        string token = "15239.20dUQQYmlHxbOPLzb" /*"30787.hZOYna2xwcSHHqk3q"*/);
}