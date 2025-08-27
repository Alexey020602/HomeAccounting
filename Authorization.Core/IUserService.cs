using MaybeResults;

namespace Authorization.Core;

public interface IUserService
{
    Task<bool> CheckLoginExist(string login, CancellationToken cancellation = default);
    Task<IMaybe<User>> GetUserByRequest(UserRequest request,
        Func<RefreshToken> createRefreshToken,
        CancellationToken cancellation = default);

    Task<IMaybe<User>> GetUserByRefreshToken(
        string refreshToken,
        Func<RefreshToken> createRefreshToken,
        CancellationToken cancellation = default
    );

    Task<IMaybe> AddUser(UnregisteredUser user, string password, CancellationToken cancellation = default);

    Task<IMaybe<User>> GetById(Guid id, CancellationToken cancellation = default);
    Task<IMaybe<IReadOnlyCollection<User>>> GetUsersByIds(IEnumerable<Guid> ids, CancellationToken cancellation = default);
    Task<IMaybe> UpdateUser(Guid userId, Contracts.UpdatedUserDto user, CancellationToken cancellation = default);
}