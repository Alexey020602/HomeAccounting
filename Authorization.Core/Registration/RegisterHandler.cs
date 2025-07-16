using LightResults;
using Mediator;

namespace Authorization.Core.Registration;

public sealed class RegisterHandler(IUserService userService) : ICommandHandler<RegisterCommand, Result>
{
    public ValueTask<Result> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        return new ValueTask<Result>(userService.AddUser(
            new UnregisteredUser()
            {
                Login = command.Login,
                UserName = command.Username,
            },
            command.Password));
    }
}