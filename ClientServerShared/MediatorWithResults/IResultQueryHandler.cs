namespace ClientServerShared.MediatorWithResults;

public interface IResultQueryHandler<in TQuery, TResult>: IQueryHandler<TQuery, IMaybe<TResult>> 
    where TQuery : IResultQuery<TResult>;