using Core.Model.ChecksList;
using Core.Model.Requests;
using Refit;

namespace Client;

public interface IChecksApi
{
    [Get("/checks")]
    Task<List<Check>> GetChecks();

    [Post("/receipt")]
    Task<Check> GetReceipt(CheckRequest checkRequest);

    // [Headers("Access-Control-Allow-Origin: *")]
    [Post("/receiptWithFile")]
    [Multipart]
    Task<Check> GetReceipt(FileCheckRequest fileCheckRequest);
}