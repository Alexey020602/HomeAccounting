using Shared.Model.Requests;

namespace Fns.Contracts;

public interface ICheckSource
{
    Task<NormalizedCheck> GetCheck(CheckRequest request);
}