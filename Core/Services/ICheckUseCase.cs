using Core.Model;
using Core.Model.ChecksList;
using Core.Model.Requests;

namespace Core.Services;

public interface ICheckUseCase
{
    Task<Check> SaveCheck(RawCheckRequest request, User user) => SaveCheck(new CheckRequest(request), user);
    Task<Check> SaveCheck(CheckRequest checkRequest, User user);
    Task<IReadOnlyList<Check>> GetChecksAsync(int skip = 0, int take = 100);
}