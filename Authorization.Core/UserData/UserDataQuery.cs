using MaybeResults;
using Mediator;
using Shared.Utils.MediatorWithResults;

namespace Authorization.Core.UserData;

public record UserDataQuery(Guid Id): IResultQuery<Contracts.User>;