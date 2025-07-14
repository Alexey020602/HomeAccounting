using Authorization.Contracts;
using Mediator;

namespace Authorization.Core.CheckLoginExist;

public sealed class CheckLoginExistHandler(IUserService userService): IQueryHandler<CheckLoginExistQuery, bool>
{
    public async ValueTask<bool> Handle(CheckLoginExistQuery query, CancellationToken cancellationToken)
    {
        return await userService.CheckLoginExist(query.Login);
    }
}