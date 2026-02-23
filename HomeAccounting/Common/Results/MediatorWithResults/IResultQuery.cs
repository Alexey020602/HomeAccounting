using MaybeResults;
using Mediator;

namespace Shared.Utils.MediatorWithResults;

public interface IResultQuery<out TResponse> : IQuery<IMaybe<TResponse>>;