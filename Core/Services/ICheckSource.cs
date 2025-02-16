using Core.Model;
using Core.Model.Normalized;
using Core.Model.Requests;

namespace Core.Services;

public interface ICheckSource
{
    Task<NormalizedCheck> GetCheck(CheckRequest request);
}