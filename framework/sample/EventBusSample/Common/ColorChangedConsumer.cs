using Light.MassTransit.RabbitMQ;
using MassTransit;

namespace EventBusSample.Common;

public class ColorChangedConsumer(
    ILogger<ColorChangedConsumer> logger) :
    IConsumer<ColorChangedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ColorChangedIntegrationEvent> context)
    {
        var message = context.Message;

        ArgumentNullException.ThrowIfNull(message, nameof(ColorChangedIntegrationEvent));

        await Task.Delay(2000);

        logger.LogInformation("Color changed from {oldColor} to {newColor} on {date} by {Id}",
            message.OldColor, message.NewColor, message.ChangeOn, message.Id);
    }
}

internal class ColorChangedConsumerDefinition :
    ConsumerDefinition<ColorChangedIntegrationEvent, ColorChangedConsumer>
{
    public ColorChangedConsumerDefinition()
    {
        // limit the number of messages consumed concurrently
        // this applies to the consumer only, not the endpoint
        ConcurrentMessageLimit = 2;
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator configurator,
        IConsumerConfigurator<ColorChangedConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        // configure message retry with millisecond intervals
        configurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));

        // use the outbox to prevent duplicate events from being published
        configurator.UseInMemoryOutbox(context);
    }
}
