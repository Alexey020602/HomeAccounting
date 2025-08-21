using MaybeResults;
using Shared.Utils.Results;

namespace Authorization.Contracts;

public interface IUsersService
{
    public Task<IMaybe<User>> GetUser(Guid id);
    public Task<IMaybe<IReadOnlyCollection<User>>> GetUsers(UserCollectionQuery query);
}

public record UserCollectionQuery(IReadOnlyCollection<Guid> Ids);

[None]
public partial record UserNotFoundError: INotFoundError;