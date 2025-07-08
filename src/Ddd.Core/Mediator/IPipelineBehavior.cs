namespace Light.Mediator;

public interface IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(
        TRequest request,
        Func<CancellationToken, Task<TResponse>> next,
        CancellationToken cancellationToken);
}
