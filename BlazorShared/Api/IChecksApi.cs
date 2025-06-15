using BlazorShared.Api.Attributes;
using Checks.Contracts;
using Refit;
using Shared.Model.Checks;
using Shared.Model.Requests;


namespace BlazorShared.Api;

[Headers("Authorization: Bearer")]
[ApiAuthorizable("checks")]
public interface IChecksApi
{
    [Get("/")]
    Task<List<CheckDto>> GetChecks();

    [Post("/")]
    Task GetReceipt(CheckRequest checkRequest);

    [Post("/file")]
    [Multipart]
    Task GetReceipt(Stream file, DateTimeOffset addedDate);
    
    Task GetReceipt(FileCheckRequest request) => GetReceipt(request.FileStream, request.AddedTime);
}