using Checks.Contracts;
using Checks.Core.Mappers;
using Fns.Contracts;
using Shared.Model.Checks;

namespace Checks.Core;

public class CheckUseCase(
    ICheckRepository checkRepository,
    ICheckSource checkSource) : ICheckUseCase
{
    public async Task<IReadOnlyList<Contracts.CheckDto>> GetChecksAsync(GetChecksQuery getChecksQuery) =>
        (await checkRepository.GetChecksAsync(getChecksQuery)).Select(check => check.ConvertToCheckList()).ToList();

    public async Task<IReadOnlyList<Category>> GetCategoriesAsync(GetChecksQuery getChecksQuery) =>
        (await checkRepository.GetProductsAsync(getChecksQuery)).ConvertToCategories();

    public async Task<Contracts.CheckDto> SaveCheck(SaveCheckRequest saveCheckRequest)
    {
        if (await checkRepository.GetCheckByRequest(saveCheckRequest.CreateGetCheckRequestFromSaveCheckRequest()) is
            { } check)
        {
            return check.ConvertToCheckList();
        }

        var checkFromFns = await checkSource.GetCheck(saveCheckRequest.CreateCheckRequestFromSaveCheckRequest());
        
        return (await checkRepository.SaveCheck(checkFromFns.CreateFromCheckDto(saveCheckRequest.Login))).ConvertToCheckList();
    }
}