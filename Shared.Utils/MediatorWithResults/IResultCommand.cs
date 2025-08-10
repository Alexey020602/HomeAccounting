using MaybeResults;
using Mediator;

namespace Shared.Utils.MediatorWithResults;

public interface IResultCommand<out TResponse> : ICommand<IMaybe<TResponse>>;
public interface IResultCommand: ICommand<IMaybe>;