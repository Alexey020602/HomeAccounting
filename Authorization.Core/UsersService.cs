using Authorization.Contracts;
using MaybeResults;

namespace Authorization.Core;

public sealed class UsersService(IUserService userService): IUsersService
{
    public async Task<IMaybe<Contracts.User>> GetUser(Guid id) => from user in (await userService.GetById(id))
        select (Contracts.User) user;

    public async Task<IMaybe<IReadOnlyCollection<Contracts.User>>> GetUsers(UserCollectionQuery query)
    {
        return (await userService.GetUsersByIds(query.Ids))
            .Map(users => users.Select(u => (Contracts.User) u)
                .ToList());
    }
}