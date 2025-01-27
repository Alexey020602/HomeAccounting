using Core.Model;

namespace Core.Services;

public interface IReportUseCase
{
    public Task<IReadOnlyList<Check>> GetChecksAsync();
}