using HomeAccounting.Users.Data;

namespace HomeAccounting.Users.Login;

static class TokenServiceExtensions
{
    internal static string CreateTokenForUser(this ITokenProvider tokenProvider, User user) => 
        tokenProvider.CreateToken(user.GetClaims());
}