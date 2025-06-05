using BlazorShared.Api.Attributes;
using Checks.Api.Requests;
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
    Task<CheckDto> GetReceipt(CheckRequest checkRequest);

    [Post("/file")]
    [Multipart]
    Task<CheckDto> GetReceipt(Stream file, DateTimeOffset addedDate);
    
    Task<CheckDto> GetReceipt(FileCheckRequest request) => GetReceipt(request.FileStream, request.AddedTime);
}