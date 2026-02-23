using Microsoft.AspNetCore.Identity;

namespace HomeAccounting.Users.Data;

sealed class User: IdentityUser<UserId>
{
    public string FullName { get; private set; }
    public RefreshToken? RefreshToken { get; private set; }
    private User(): base()
    {
        FullName = string.Empty;
    }

    public User(string userName, string fullName): base(userName)
    {
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

    public void UpdateUserName(string userName) => UserName = userName;

    public void UpdateFullName(string fullName) => FullName = fullName;
    internal void AddRefreshToken(RefreshToken refreshToken) => RefreshToken = refreshToken; 
}
