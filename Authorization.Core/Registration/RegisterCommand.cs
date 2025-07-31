using MaybeResults;
using Mediator;

namespace Authorization.Core.Registration;

public interface IResultCommand : ICommand<IMaybe>;
public record RegisterCommand(string Login, string Username, string Password) : IResultCommand;