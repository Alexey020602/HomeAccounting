using Authorization.Contracts;
using MaybeResults;
using Mediator;
using Shared.Utils.MediatorWithResults;

namespace Authorization.Core.Login;

public record LoginQuery(string Login, string Password) : IResultRequest<LoginResponse>;