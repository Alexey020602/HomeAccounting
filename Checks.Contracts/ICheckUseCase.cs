namespace Checks.Contracts;

public interface ICheckUseCase
{
    Task<Contracts.CheckDto> SaveCheck(SaveCheckRequest saveCheckRequest);
    Task<IReadOnlyList<Contracts.CheckDto>> GetChecksAsync(GetChecksQuery getChecksQuery);
    
    Task<IReadOnlyList<Category>> GetCategoriesAsync(GetChecksQuery getChecksQuery);
}

// public static class CheckUseCaseExtensions
// {
//     public static Task<Check> SaveCheck(this ICheckUseCase checkUseCase, RawCheckRequest request, User user) => checkUseCase.SaveCheck(new CheckRequest(request), user);
// }