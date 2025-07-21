using System.Security.Claims;

namespace Authorization.Core;

public static class ClaimsConstants
{
    public const string Fullname = "FullName";
}
public static class UserExtensions
{
    public static IReadOnlyList<Claim> GetClaims(this Core.User user) =>
    [
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName ?? throw UserException.NoUserName),
        new Claim(ClaimsConstants.Fullname, user.FullName ?? throw UserException.NoFullName)
    ];
}

public sealed class UserException : Exception
{
    public UserException(string message) : base(message) { }
    public UserException(string message, Exception innerException) : base(message, innerException) { }

    public static UserException NoFullName => new ("Отсутствует имя пользователя");
    public static UserException NoUserName => new ("Имя пользователя отсутствует");
}