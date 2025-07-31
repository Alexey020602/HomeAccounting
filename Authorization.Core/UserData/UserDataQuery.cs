using LightResults;
using MaybeResults;
using Mediator;

namespace Authorization.Core.UserData;

public record UserDataQuery(Guid Id): IQuery<IMaybe<Contracts.User>>;