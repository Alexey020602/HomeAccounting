using Microsoft.AspNetCore.Identity;

namespace MyBudgets.Users.Data;

sealed class User: IdentityUser<UserId>
{
    public string FullName { get; private set; }
    public RefreshToken? RefreshToken { get; private set; }
    private User(): base()
    {
        FullName = string.Empty;
    }

    public User(string userName, string fullName)
    {
        UserName = userName;
        FullName = fullName;
    }

    internal static User CreateForSeeding(Guid userId, string userName, string fullName, string password)
    {
        var user = new User()
        {
            Id = new(userId),
            UserName = userName,
            FullName = fullName,
            NormalizedUserName = userName.ToUpper(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
        };
        user.PasswordHash =  new PasswordHasher<User>().HashPassword(user, password);
        return user;
    }

    internal void AddRefreshToken(RefreshToken refreshToken) => RefreshToken = refreshToken; 
}

// sealed class RefreshToken
// {
//     public UserId UserId { get; init; }
//     public string Token { get; init; }
// }