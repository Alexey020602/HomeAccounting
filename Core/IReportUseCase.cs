using Core.Model;
using DataBase;

namespace Core;

public interface IReportUseCase
{
    public Task<IReadOnlyList<Check>> GetChecksAsync();
}