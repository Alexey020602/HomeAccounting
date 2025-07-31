using LightResults;
using MaybeResults;
using Mediator;

namespace Authorization.Core.UserData;

public sealed class UserDataHandler(IUserService userService): IQueryHandler<UserDataQuery, IMaybe<Contracts.User>>
{
    public async ValueTask<IMaybe<Contracts.User>> Handle(UserDataQuery query, CancellationToken cancellationToken)
    {
        return (await userService.GetById(query.Id, cancellationToken)).FlatMap(u => Maybe.Create<Contracts.User>(u));
    }
}