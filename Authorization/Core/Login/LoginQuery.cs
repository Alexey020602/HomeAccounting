using Authorization.Contracts;
using LightResults;
using Mediator;

namespace Authorization.Core.Login;

public record LoginQuery(string Login, string Password) : IRequest<Result<AuthorizationResponse>>;