using MaybeResults;
using Mediator;
using Shared.Utils.MediatorWithResults;

namespace Authorization.Core.UserData;

public sealed class UserDataHandler(IUserService userService): IResultQueryHandler<UserDataQuery, Contracts.User>
{
    public async ValueTask<IMaybe<Contracts.User>> Handle(UserDataQuery query, CancellationToken cancellationToken)
    {
        return (await userService.GetById(query.Id, cancellationToken)).FlatMap(u => Maybe.Create<Contracts.User>(u));
    }
}