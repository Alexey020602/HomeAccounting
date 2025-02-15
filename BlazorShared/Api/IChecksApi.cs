using BlazorShared.Api.Attributes;
using Core.Model.ChecksList;
using Core.Model.Requests;
using Refit;

namespace BlazorShared.Api;

[Api("checks")]
public interface IChecksApi
{
    [Get("/")]
    Task<List<Check>> GetChecks();

    [Post("/")]
    Task<Check> GetReceipt(CheckRequest checkRequest);

    [Post("/file")]
    [Multipart]
    Task<Check> GetReceipt(Stream file, DateTimeOffset addedDate);
    
    Task<Check> GetReceipt(FileCheckRequest request) => GetReceipt(request.FileStream, request.AddedTime);
}