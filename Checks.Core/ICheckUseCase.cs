using Checks.Contracts;
using Shared.Model;
using Shared.Model.Checks;

namespace Checks.Core;

public interface ICheckUseCase
{
    Task<Contracts.CheckDto> SaveCheck(SaveCheckRequest saveCheckRequest);
    Task<IReadOnlyList<Contracts.CheckDto>> GetChecksAsync(int skip = 0, int take = 100);
}

// public static class CheckUseCaseExtensions
// {
//     public static Task<Check> SaveCheck(this ICheckUseCase checkUseCase, RawCheckRequest request, User user) => checkUseCase.SaveCheck(new CheckRequest(request), user);
// }