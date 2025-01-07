using FnsChecksApi.Requests;
using Refit;

namespace Client;

public interface IApi
{
    [Get("/test")]
    Task<string> GetTest();
    [Post("/test")]
    Task<string> PostTest([Body] string data);
    [Post("/receipt")]
    Task<FnsChecksApi.Dto.Categorized.Root> GetReceipt(CheckRequest checkRequest);
    
    // [Headers("Access-Control-Allow-Origin: *")]
    [Post("/receiptWithFile")]
    [Multipart]
    Task<FnsChecksApi.Dto.Categorized.Root> GetReceipt(Stream file);
}