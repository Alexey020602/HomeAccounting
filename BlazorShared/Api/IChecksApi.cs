using Core.Model.ChecksList;
using Core.Model.Requests;
using Refit;

namespace BlazorShared.Api;

public interface IChecksApi
{
    [Get("/checks")]
    Task<List<Check>> GetChecks();

    [Post("/checks")]
    Task<Check> GetReceipt(CheckRequest checkRequest);

    [Post("/checks/file")]
    [Multipart]
    Task<Check> GetReceipt(Stream file, DateTimeOffset addedDate);
    
    Task<Check> GetReceipt(FileCheckRequest request) => GetReceipt(request.FileStream, request.AddedTime);
}