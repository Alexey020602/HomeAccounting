using Shared.Model.Checks;

namespace Checks.Core;

public class CheckUseCase(
    ICheckRepository checkRepository,
    ICheckSource checkSource) : ICheckUseCase
{
    public Task<IReadOnlyList<Check>> GetChecksAsync(int skip = 0, int take = 100) =>
        checkRepository.GetChecksAsync(skip, take);

    public async Task<Check> SaveCheck(SaveCheckRequest saveCheckRequest)
    {
        return await checkRepository.GetCheckByRequest(saveCheckRequest.CreateGetCheckRequestFromSaveCheckRequest()) ??
               await checkRepository.SaveCheck(
                   (await checkSource.GetCheck(saveCheckRequest.CreateGetCheckRequestFromSaveCheckRequest()))
                   .CreateFromCheckDto(saveCheckRequest.User)
               );
    }
}