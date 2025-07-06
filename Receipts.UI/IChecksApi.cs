using Receipts.Contracts;
using Refit;
using Shared.Blazor.Attributes;

namespace Receipts.UI;

[Headers("Authorization: Bearer")]
[ApiAuthorizable("receipts")]
public interface IChecksApi
{
    [Get("/")]
    Task<List<CheckDto>> GetChecks();

    [Put("/")]
    Task<CheckDto> GetReceipt(CheckRequest checkRequest);

    [Put("/file")]
    [Multipart]
    Task<CheckDto> GetReceipt(Stream file);
    
    // Task<CheckDto> GetReceipt(FileCheckRequest request) => GetReceipt(request.FileStream, request.AddedTime);
}