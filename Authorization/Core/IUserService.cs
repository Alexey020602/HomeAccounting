using Authorization.DataBase;
using LightResults;

namespace Authorization.Core;

public interface IUserService
{
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