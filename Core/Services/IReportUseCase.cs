using Core.Model.ChecksList;

namespace Core.Services;

public interface IReportUseCase
{
    public Task<IReadOnlyList<Check>> GetChecksAsync(int skip = 0, int take = 100);
}