using Light.MassTransit.RabbitMQ;
using MassTransit;

namespace EventBusConsumer.Common;

public class ColorChangedConsumer(
    ILogger<ColorChangedConsumer> logger) :
    Consumer<ColorChangedIntegrationEvent>(logger)
{
    public override bool ThrowIfError => true;

    public override async Task Handle(ColorChangedIntegrationEvent message)
    {
        await Task.Delay(1000);

        logger.LogInformation("Color changed from {oldColor} to {newColor} on {date} by {Id}",
            message.OldColor, message.NewColor, message.ChangeOn, message.Id);

        //throw new Exception("Color changed error when empty");
    }
}

internal class ColorChangedConsumerDefinition :
    ConsumerDefinition<ColorChangedIntegrationEvent, ColorChangedConsumer>
{
    public ColorChangedConsumerDefinition()
    {
        // limit the number of messages consumed concurrently
        // this applies to the consumer only, not the endpoint
        ConcurrentMessageLimit = 10;
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator configurator,
        IConsumerConfigurator<ColorChangedConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        // configure message retry with millisecond intervals
        configurator.UseMessageRetry(r => r.Intervals(
            TimeSpan.FromMinutes(10),
            TimeSpan.FromMinutes(30),
            TimeSpan.FromHours(1),
            TimeSpan.FromHours(2),
            TimeSpan.FromHours(4)));

        // use the outbox to prevent duplicate events from being published
        configurator.UseInMemoryOutbox(context);
    }
}
