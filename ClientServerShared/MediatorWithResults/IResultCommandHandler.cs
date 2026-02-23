namespace ClientServerShared.MediatorWithResults;

public interface IResultCommandHandler<in TCommand, TResult>: ICommandHandler<TCommand, IMaybe<TResult>> where TCommand: IResultCommand<TResult>;
public interface IResultCommandHandler<in TCommand> : ICommandHandler<TCommand, IMaybe> where TCommand : IResultCommand;