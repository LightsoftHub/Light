using Light.Domain;
using Light.EventBus.Events;

namespace EventBusConsumer.Common;

public abstract record IntegrationEvent : IIntegrationEvent
{
    protected IntegrationEvent()
    {
        Id = LightId.NewId();
        CreationDate = DateTime.UtcNow;
    }

    public string Id { get; set; }

    public DateTime CreationDate { get; set; }
}

public abstract record EventBase : IntegrationEvent
{
    public int Value = 1000;
}
