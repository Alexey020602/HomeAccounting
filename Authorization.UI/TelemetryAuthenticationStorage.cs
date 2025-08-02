using Authorization.UI.Dto;
using Microsoft.Extensions.Logging;

namespace Authorization.UI;

public sealed class TelemetryAuthenticationStorage(
    IAuthenticationStorage authenticationStorage,
    ILogger<TelemetryAuthenticationStorage> logger
) : IAuthenticationStorage
{
    public async ValueTask RemoveAuthorizationAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Removing authorization");
            await authenticationStorage.RemoveAuthorizationAsync(cancellationToken);
            logger.LogInformation("Authorization removed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error removing authorization");
            throw;
        }
    }

    public async ValueTask SetAuthorizationAsync(Authentication authorization, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Adding authorization");
            await authenticationStorage.SetAuthorizationAsync(authorization, cancellationToken);
            logger.LogInformation("Authorization added successfully");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error setting authorization");
            throw;
        }
    }

    public async ValueTask<Authentication?> GetAuthorizationAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Getting authorization");
            var authentication = await authenticationStorage.GetAuthorizationAsync(cancellationToken);
            logger.LogInformation("Authorization retrieved successfully");
            return authentication;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting authorization");
            throw;
        }
    }
}