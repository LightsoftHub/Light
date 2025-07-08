using Light.Mediator;

namespace Sample.Mediator;

public class ValueUpdated
{
    public record Event(string Value) : INotification;

    public class Handler(ILogger<Handler> logger) : INotificationHandler<Event>
    {
        public async Task Handle(Event notification, CancellationToken cancellationToken)
        {
            await Task.Delay(2000, cancellationToken);
            
            logger.LogInformation("Value updated: {Value}", notification.Value);
        }
    }
}