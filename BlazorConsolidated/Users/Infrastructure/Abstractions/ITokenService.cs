namespace BlazorConsolidated.Users.Infrastructure.Abstractions;

public interface ITokenService
{
    Task<string?> GetFreshAccessToken(CancellationToken cancellationToken = default);
    Task<string> GetRefreshedToken(CancellationToken cancellationToken = default);
}