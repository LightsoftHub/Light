namespace Light.Mediator;

public class MediatorImp(IServiceProvider provider) : IMediator
{
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var responseType = typeof(TResponse);

        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);

        // The delegate that calls the handler directly
        Func<CancellationToken, Task<TResponse>> handlerDelegate = ct =>
        {
            var resultTask = (Task<TResponse>)handlerType.InvokeHandleMethod(
                provider,
                [request, ct]
            )!;
            return resultTask;
        };

        // Find pipeline behaviors
        var behaviorsType = typeof(IEnumerable<>).MakeGenericType(
            typeof(IPipelineBehavior<,>).MakeGenericType(requestType, responseType));

        var behaviors = (IEnumerable<object>)
            (provider.GetService(behaviorsType) ?? Enumerable.Empty<object>());

        // Chain pipeline behaviors in reverse order
        foreach (var behavior in behaviors.Reverse())
        {
            var behaviorType = behavior.GetType();

            var next = handlerDelegate;

            handlerDelegate = ct =>
            {
                var resultTask = (Task<TResponse>)behaviorType.InvokeHandleMethod(
                    behavior,
                    [request, next, ct]
                )!;
                return resultTask;
            };
        }

        return await handlerDelegate(cancellationToken);
    }

    public Task Publish(INotification notification, CancellationToken cancellationToken = default)
    {
        return (Task)typeof(INotificationHandler<>)
            .MakeGenericType(notification.GetType())
            .InvokeHandleMethod(provider, [notification, cancellationToken]);
    }
}
