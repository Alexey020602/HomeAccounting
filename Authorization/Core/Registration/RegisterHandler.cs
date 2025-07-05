using LightResults;
using Mediator;

namespace Authorization.Core.Registration;

public sealed class RegisterHandler(IAuthenticationManager authenticationManager) : ICommandHandler<RegisterCommand, Result>
{
    public ValueTask<Result> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        return new ValueTask<Result>(authenticationManager.Register(command));
    }
}