namespace ClientServerShared.MediatorWithResults;

public interface IResultQuery<out TResponse> : IQuery<IMaybe<TResponse>>;