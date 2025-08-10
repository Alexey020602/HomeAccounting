using MaybeResults;
using Mediator;

namespace Shared.Utils.MediatorWithResults;

public interface IResultRequestHandler<in TRequest, TResult> : IRequestHandler<TRequest, IMaybe<TResult>>
    where TRequest : IResultRequest<TResult>;

public interface IResultRequestHandler<in TRequest> : IRequestHandler<TRequest, IMaybe> where TRequest : IResultRequest;