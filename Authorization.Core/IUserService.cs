using Authorization.Contracts;
using LightResults;

namespace Authorization.Core;

public interface IUserService
{
    Task<bool> CheckLoginExist(string login);
    Task<Result<Core.User>> GetUserByRequest(
        UserRequest request, 
        Func<Core.RefreshToken> createRefreshToken
    );

    Task<Result<Core.User>> GetUserByRefreshToken(
        string refreshToken,
        Func<Core.RefreshToken> createRefreshToken
    );

    Task<Result> AddUser(UnregisteredUser user, string password);
}