using Core.Model.ChecksList;
using Core.Model.Requests;

namespace Core.Services;

public interface ICheckUseCase
{
    // Task<FnsChecksApi.Dto.Categorized.Root> GetReceipt(CheckRequest checkRequest);
    // Task<FnsChecksApi.Dto.Categorized.Root> GetReceipt(Stream file);
    // Task<FnsChecksApi.Dto.Categorized.Root> GetReceipt(FileInfo file);
    // Task<FnsChecksApi.Dto.Categorized.Root> GetReceipt(string qrRaw);
    Task<Check> SaveCheck(RawCheckRequest request)
    {
        return SaveCheck(new CheckRequest(request));
    }

    Task<Check> SaveCheck(CheckRequest checkRequest);
}