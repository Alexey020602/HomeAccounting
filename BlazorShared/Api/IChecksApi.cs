using BlazorShared.Api.Attributes;
using Receipts.Contracts;
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

    [Put("/")]
    Task<CheckDto> GetReceipt(CheckRequest checkRequest);

    [Put("/file")]
    [Multipart]
    Task<CheckDto> GetReceipt(Stream file, DateTimeOffset addedDate);
    
    Task<CheckDto> GetReceipt(FileCheckRequest request) => GetReceipt(request.FileStream, request.AddedTime);
}