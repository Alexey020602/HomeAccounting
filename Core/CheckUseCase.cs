using Core.Model;
using Core.Services;
using CheckRequest = Core.Model.Requests.CheckRequest;

namespace Core;

public class CheckUseCase(
    ICheckRepository checkRepository,
    ICheckSource checkSource) : ICheckUseCase
{
    public Task<IReadOnlyList<Model.ChecksList.Check>> GetChecksAsync(int skip = 0, int take = 100) =>
        checkRepository.GetChecksAsync(skip, take);

    public async Task<Model.ChecksList.Check> SaveCheck(CheckRequest checkRequest, User user)
    {
        return (await checkRepository.GetCheckByRequest(checkRequest)) ??
               await checkRepository.SaveCheck(await checkSource.GetCheck(checkRequest), user);
    }
}