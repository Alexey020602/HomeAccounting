using Authorization.Contracts;
using MaybeResults;
using Mediator;

namespace Authorization.Core.Login;

public interface IMaybeRequest<out TResponse> : IRequest<IMaybe<TResponse>>;
public record LoginQuery(string Login, string Password) : IMaybeRequest<AuthorizationResponse>;