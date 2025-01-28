using Core.Extensions;
using Core.Model;

namespace Core.Services;

public interface ICheckUseCase
{
    // Task<FnsChecksApi.Dto.Categorized.Root> GetReceipt(CheckRequest checkRequest);
    // Task<FnsChecksApi.Dto.Categorized.Root> GetReceipt(Stream file);
    // Task<FnsChecksApi.Dto.Categorized.Root> GetReceipt(FileInfo file);
    // Task<FnsChecksApi.Dto.Categorized.Root> GetReceipt(string qrRaw);
    Task<Check> SaveCheck(string qrRaw) => SaveCheck(new CheckRequest(qrRaw));
    Task<Check> SaveCheck(CheckRequest checkRequest);
} 