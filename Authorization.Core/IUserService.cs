using LightResults;

namespace Authorization.Core;

public interface IUserService
{
    Task<bool> CheckLoginExist(string login);
    Task<Result<User>> GetUserByRequest(
        UserRequest request, 
        Func<RefreshToken> createRefreshToken
    );

    Task<Result<User>> GetUserByRefreshToken(
        string refreshToken,
        Func<RefreshToken> createRefreshToken
    );

    Task<Result> AddUser(UnregisteredUser user, string password);
}