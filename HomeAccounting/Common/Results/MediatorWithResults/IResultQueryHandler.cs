using MaybeResults;
using Mediator;

namespace Shared.Utils.MediatorWithResults;

public interface IResultQueryHandler<in TQuery, TResult>: IQueryHandler<TQuery, IMaybe<TResult>> 
    where TQuery : IResultQuery<TResult>;