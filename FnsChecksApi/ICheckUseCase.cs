using FnsChecksApi.Requests;

namespace FnsChecksApi;

public interface ICheckUseCase
{
    Task<Dto.Categorized.Root> GetReceipt(CheckRequest checkRequest);
    Task<Dto.Categorized.Root> GetReceipt(Stream file);
    Task<Dto.Categorized.Root> GetReceipt(FileInfo file);
    Task<Dto.Categorized.Root> GetReceipt(CheckRawRequest checkRequest);
}