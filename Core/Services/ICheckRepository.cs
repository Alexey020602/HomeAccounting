using Core.Model;
using Core.Model.ChecksList;
using Core.Model.Normalized;
using Core.Model.Requests;

namespace Core.Services;

public interface ICheckRepository
{
    Task<Check> SaveCheck(NormalizedCheck normalizedCheck);

    Task<Check?> GetCheckByRequest(CheckRequest checkRequest);
    Task<IReadOnlyList<Check>> GetChecksAsync(int skip = 0, int take = 100);
}