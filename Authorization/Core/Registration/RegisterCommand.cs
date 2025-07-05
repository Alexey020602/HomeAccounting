using LightResults;
using Mediator;

namespace Authorization.Core.Registration;

public record RegisterCommand(string Login, string Username, string Password) : ICommand<Result>;