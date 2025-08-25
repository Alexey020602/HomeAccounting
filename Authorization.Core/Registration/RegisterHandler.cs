using MaybeResults;
using Mediator;
using Shared.Utils.MediatorWithResults;

namespace Authorization.Core.Registration;

public sealed class RegisterHandler(IUserService userService) : IResultCommandHandler<RegisterCommand>
{
    public ValueTask<IMaybe> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        return new ValueTask<IMaybe>(userService.AddUser(
            new UnregisteredUser()
            {
                Login = command.Login,
                UserName = command.Username,
            },
            command.Password, cancellationToken));
    }
}