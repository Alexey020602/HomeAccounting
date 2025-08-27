using MaybeResults;
using Shared.Utils.MediatorWithResults;

namespace Authorization.Core.UpdateUser;

public sealed class UpdateUserCommandHandler(IUserService userService): IResultCommandHandler<UpdateUserCommand>
{
    public ValueTask<IMaybe> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        return new(userService.UpdateUser(command.Id, command.User, cancellationToken));
    }
}