using MaybeResults;
using Mediator;
using Shared.Utils.MediatorWithResults;

namespace Authorization.Core.Registration;

public record RegisterCommand(string Login, string Username, string Password) : IResultCommand;