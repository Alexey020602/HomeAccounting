using Core.Model;
using FnsChecksApi.Requests;

namespace Core;

public interface ICheckUseCase
{
    Task<FnsChecksApi.Dto.Categorized.Root> GetReceipt(CheckRequest checkRequest);
    Task<FnsChecksApi.Dto.Categorized.Root> GetReceipt(Stream file);
    Task<FnsChecksApi.Dto.Categorized.Root> GetReceipt(FileInfo file);
    Task<FnsChecksApi.Dto.Categorized.Root> GetReceipt(CheckRawRequest checkRequest);
    Task<Check> SaveCheck(CheckRawRequest checkRequest);
}