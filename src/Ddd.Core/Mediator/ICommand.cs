namespace Light.Mediator;

public interface ICommand<TResponse> : IRequest<TResponse>;

public interface ICommandHandler<in TRequest, TResponse>
    : IRequestHandler<TRequest, TResponse>
    where TRequest : ICommand<TResponse>;