using BlazorShared.Api.Attributes;
using Checks.Api.Requests;
using Refit;
using Shared.Model.Checks;
using Shared.Model.Requests;


namespace BlazorShared.Api;

[Headers("Authorization: Bearer")]
[ApiAuthorizable("checks")]
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