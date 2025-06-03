using Shared.Model.Requests;

namespace Shared.Model.NormalizedChecks;

public interface ICheckSource
{
    Task<NormalizedCheck> GetCheck(CheckRequest request);
}