using LightResults;
using Mediator;

namespace Authorization.Core.UserData;

public record UserDataQuery(Guid Id): IQuery<Result<Contracts.User>>;