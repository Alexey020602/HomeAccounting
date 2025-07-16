using Authorization.Extensions;

namespace Authorization.Core;

public static class TokenServiceExtensions
{
    internal static string CreateTokenForUser(this ITokenService tokenService, User user) => 
        tokenService.CreateToken(user.GetClaims());
}