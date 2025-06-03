using Shared.Model;
using Shared.Model.Checks;

namespace Checks.Core;

public interface ICheckUseCase
{
    Task<Check> SaveCheck(SaveCheckRequest saveCheckRequest);
    Task<IReadOnlyList<Check>> GetChecksAsync(int skip = 0, int take = 100);
}

// public static class CheckUseCaseExtensions
// {
//     public static Task<Check> SaveCheck(this ICheckUseCase checkUseCase, RawCheckRequest request, User user) => checkUseCase.SaveCheck(new CheckRequest(request), user);
// }