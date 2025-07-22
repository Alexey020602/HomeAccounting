using System.Security.Claims;
using System.Text.Json.Serialization;
using Shared.Utils.Model;

namespace Authorization.Contracts;

[method: JsonConstructor]
public record User(Guid Id, string UserName)
{
    // public static operator explicit ClaimsPrincipal (User user) => new ClaimsPrincipal(new ClaimsIdentity(user.GetClaims()));
    public static explicit operator ClaimsPrincipal(User user) =>
        new ClaimsPrincipal(new ClaimsIdentity(user.GetClaims(), ClaimsIdentityConstants.AuthenticationType));

    public static explicit operator User(ClaimsPrincipal principal) => new User(
        principal.GetUserId(),
        principal.GetUserName() ?? throw UserException.NoUserName
    );

    public IReadOnlyList<Claim> GetClaims() =>
    [
        new(ClaimTypes.NameIdentifier, Id.ToString()),
        new(ClaimTypes.Name, UserName)
    ];
}

public static class ClaimsPrincipalExtensions
{
    // public static string? GetUserId(this ClaimsPrincipal principal) =>
    // principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public static string? GetUserName(this ClaimsPrincipal principal) => principal.FindFirst(ClaimTypes.Name)?.Value;
}