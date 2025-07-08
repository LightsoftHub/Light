namespace Light.Mediator;

public interface IPublisher
{
    Task Publish(INotification notification, CancellationToken cancellationToken = default);
}
