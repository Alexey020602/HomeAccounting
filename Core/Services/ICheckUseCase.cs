using Core.Model.ChecksList;
using Core.Model.Requests;

namespace Core.Services;

public interface ICheckUseCase
{
    Task<Check> SaveCheck(RawCheckRequest request) => SaveCheck(new CheckRequest(request));
    Task<Check> SaveCheck(CheckRequest checkRequest);
    Task<IReadOnlyList<Check>> GetChecksAsync(int skip = 0, int take = 100);
}