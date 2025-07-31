using LightResults;
using MaybeResults;

namespace Authorization.Core;

public interface IUserService
{
    Task<bool> CheckLoginExist(string login, CancellationToken cancellation = default);
    Task<Result<User>> GetUserByRequest(
        UserRequest request, 
        Func<RefreshToken> createRefreshToken, 
        CancellationToken cancellation = default
    );

    Task<Result<User>> GetUserByRefreshToken(
        string refreshToken,
        Func<RefreshToken> createRefreshToken,
        CancellationToken cancellation = default
    );

    Task<Result> AddUser(UnregisteredUser user, string password, CancellationToken cancellation = default);

    Task<IMaybe<User>> GetById(Guid id, CancellationToken cancellation = default);
}