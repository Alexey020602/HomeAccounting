using Checks.Contracts;
using Fns.Contracts;
using Shared.Model.Checks;

namespace Checks.Core;

public class CheckUseCase(
    ICheckRepository checkRepository,
    ICheckSource checkSource) : ICheckUseCase
{
    public Task<IReadOnlyList<Contracts.CheckDto>> GetChecksAsync(int skip = 0, int take = 100) =>
        checkRepository.GetChecksAsync(skip, take);

    public async Task<Contracts.CheckDto> SaveCheck(SaveCheckRequest saveCheckRequest)
    {
        return await checkRepository.GetCheckByRequest(saveCheckRequest.CreateGetCheckRequestFromSaveCheckRequest()) ??
               await checkRepository.SaveCheck(
                   (await checkSource.GetCheck(saveCheckRequest.CreateCheckRequestFromSaveCheckRequest()))
                   .CreateFromCheckDto(saveCheckRequest.Login)
               );
    }
}