using System.Security.Claims;
using ClientServerContracts.Users.GetUser;

namespace ClientServerShared.Users;

public static class UserExtensions
{
    public static ClaimsPrincipal GetPrincipal(this User user) =>
        new ClaimsPrincipal(new ClaimsIdentity(user.GetClaims(), ClaimsIdentityConstants.AuthenticationType));

    public static User CreateUser(this ClaimsPrincipal principal) => new User(
        principal.GetUserId(),
        principal.GetUserName() ?? throw UserException.NoUserName,
        principal.GetFullName() ?? throw UserException.NoFullName
    );

    public static IReadOnlyList<Claim> GetClaims(this User user) =>
    [
        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new(ClaimTypes.Name, user.UserName),
        new (ClaimsConstants.FullName, user.FullName)
    ];
}