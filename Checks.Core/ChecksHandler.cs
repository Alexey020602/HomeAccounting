using Checks.Contracts;
using Wolverine.Attributes;

namespace Checks.Core;
// public record GetChecks(GetChecksQuery Query);
public record ChecksResponse(IReadOnlyList<CheckDto> Checks);
public static class ChecksHandler
{
    public static async Task<ChecksResponse> Handle(GetChecksQuery query, ICheckUseCase checkUseCase)
    {
        return new ChecksResponse(await checkUseCase.GetChecksAsync(query));
    }
}