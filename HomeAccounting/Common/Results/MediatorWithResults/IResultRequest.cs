using MaybeResults;
using Mediator;

namespace Shared.Utils.MediatorWithResults;

public interface IResultRequest<out TResponse> : IRequest<IMaybe<TResponse>>;

public interface IResultRequest : IRequest<IMaybe>;

