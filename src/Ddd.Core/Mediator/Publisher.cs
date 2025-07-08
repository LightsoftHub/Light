using Microsoft.Extensions.DependencyInjection;

namespace Light.Mediator;

public class MediatorImp(IServiceProvider provider) : IMediator
{
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));

        // Retrieve pipeline behaviors
        var behaviorsType = typeof(IEnumerable<>)
            .MakeGenericType(typeof(IPipelineBehavior<,>)
            .MakeGenericType(requestType, typeof(TResponse)));

        var behaviors = (IEnumerable<object>)
            (provider.GetService(behaviorsType) ?? Enumerable.Empty<object>());

        dynamic handler = provider.GetRequiredService(handlerType);

        Func<CancellationToken, Task<TResponse>> handlerDelegate =
                ct => handler.Handle((dynamic)request, ct);

        // Chain behaviors in reverse order
        foreach (var behavior in behaviors.Reverse())
        {
            var next = handlerDelegate;
            handlerDelegate = ct =>
            {
                dynamic b = behavior;
                return b.Handle((dynamic)request, next, cancellationToken);
            };
        }

        return await handlerDelegate(cancellationToken);
    }

    public Task Publish(INotification notification, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notification.GetType());
        dynamic handler = provider.GetRequiredService(handlerType);
        return handler.Handle((dynamic)notification, cancellationToken);
    }
}
