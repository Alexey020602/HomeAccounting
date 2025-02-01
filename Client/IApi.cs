using Core.Model;
using Core.Model.Requests;
using Refit;

namespace Client;

public interface IApi
{
    [Post("/receipt")]
    Task<Check> GetReceipt(CheckRequest checkRequest);
    
    // [Headers("Access-Control-Allow-Origin: *")]
    [Post("/receiptWithFile")]
    [Multipart]
    Task<Check> GetReceipt(Stream file);
}