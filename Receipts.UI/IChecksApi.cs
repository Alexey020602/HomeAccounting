using Receipts.Contracts;
using Refit;
using Shared.Blazor.Attributes;

namespace Receipts.UI;

[Headers("Authorization: Bearer")]
[ApiAuthorizable("budgets")]
public interface IChecksApi
{
    [Get("/{id}/receipts")]
    Task<List<CheckDto>> GetChecks(Guid id);

    [Put("/{id}/receipts")]
    Task<CheckDto> SaveReceipt(Guid id, CheckRequest checkRequest);

    [Put("/{id}/receipts/file")]
    [Multipart]
    Task<CheckDto> SaveReceipt(Guid id, Stream file);
    
    // Task<CheckDto> GetReceipt(FileCheckRequest request) => GetReceipt(request.FileStream, request.AddedTime);
}