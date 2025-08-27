using Shared.Utils.MediatorWithResults;

namespace Authorization.Core.UpdateUser;

public record UpdateUserCommand(Guid Id, Contracts.UpdatedUserDto User): IResultCommand;