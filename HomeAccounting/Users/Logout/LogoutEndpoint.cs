using System.Net;
using ClientServerContracts.Users.Logout;
using HomeAccounting.Users.Login;

namespace HomeAccounting.Users.Logout;

/// <summary>
/// Endpoints for revoking sessions (logout).
/// </summary>
static class LogoutEndpoint
{
    /// <summary>
    /// Maps POST logout. Revokes the session associated with the given refresh token. Idempotent.
    /// </summary>
    public static void MapLogout(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPost(
                "logout",
                async (LogoutRequest request, ITokenProvider tokenProvider, CancellationToken cancellationToken) =>
                {
                    await tokenProvider.LogoutUserSession(request.RefreshToken, cancellationToken);
                    return Results.NoContent();
                })
            .WithName("Logout")
            .WithTags("Users")
            .WithSummary("Logout")
            .WithDescription("Revokes the session associated with the given refresh token. Idempotent: unknown or already revoked token completes without error.")
            .Produces((int)HttpStatusCode.NoContent)
            .AllowAnonymous();
    }

    /// <summary>
    /// Maps POST logout/all. Revokes all sessions of the user identified by the given refresh token. Idempotent.
    /// </summary>
    public static void MapLogoutAll(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPost(
                "logout/all",
                async (LogoutRequest request, ITokenProvider tokenProvider, CancellationToken cancellationToken) =>
                {
                    await tokenProvider.LogoutAllUserSessions(request.RefreshToken, cancellationToken);
                    return Results.NoContent();
                })
            .WithName("LogoutAll")
            .WithTags("Users")
            .WithSummary("Logout all sessions")
            .WithDescription("Revokes all sessions of the user associated with the given refresh token. Idempotent: unknown or already revoked token completes without error.")
            .Produces((int)HttpStatusCode.NoContent)
            .AllowAnonymous();
    }
}
