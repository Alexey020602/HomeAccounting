using System.Security.Claims;
using System.Text.Json.Serialization;
using Shared.Utils.Model;

namespace Authorization.Contracts;

[method: JsonConstructor]
public record User(Guid Id, string UserName, string FullName)
{
    // public static operator explicit ClaimsPrincipal (User user) => new ClaimsPrincipal(new ClaimsIdentity(user.GetClaims()));
    public static explicit operator ClaimsPrincipal(User user) =>
        new ClaimsPrincipal(new ClaimsIdentity(user.GetClaims(), ClaimsIdentityConstants.AuthenticationType));

    public static explicit operator User(ClaimsPrincipal principal) => new User(
        principal.GetUserId(),
        principal.GetUserName() ?? throw UserException.NoUserName,
        principal.GetFullName() ?? throw UserException.NoFullName
    );

    public IReadOnlyList<Claim> GetClaims() =>
    [
        new(ClaimTypes.NameIdentifier, Id.ToString()),
        new(ClaimTypes.Name, UserName),
        new (ClaimsConstants.FullName, FullName)
    ];

    public override string ToString() => "Id:{Id}\nUserName:{UserName}\nFullName:{FullName}";
}

public static class ClaimsPrincipalExtensions
{
    // public static string? GetUserId(this ClaimsPrincipal principal) =>
    // principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public static string? GetUserName(this ClaimsPrincipal principal) => principal.FindFirst(ClaimTypes.Name)?.Value;
    public static string? GetFullName(this ClaimsPrincipal principal) => principal.FindFirst(ClaimsConstants.FullName)?.Value;
}