namespace ClientServerShared.MediatorWithResults;

public interface IResultRequest<out TResponse> : IRequest<IMaybe<TResponse>>;

public interface IResultRequest : IRequest<IMaybe>;

