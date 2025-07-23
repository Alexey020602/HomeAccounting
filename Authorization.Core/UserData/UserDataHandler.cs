using LightResults;
using Mediator;

namespace Authorization.Core.UserData;

public sealed class UserDataHandler(IUserService userService): IQueryHandler<UserDataQuery, Result<Contracts.User>>
{
    public async ValueTask<Result<Contracts.User>> Handle(UserDataQuery query, CancellationToken cancellationToken)
    {
        return (await userService.GetById(query.Id, cancellationToken)).IsFailure(out var error, out var user) 
            ? Result.Failure<Contracts.User>(error) 
            : Result.Success<Contracts.User>(user);
    }
}