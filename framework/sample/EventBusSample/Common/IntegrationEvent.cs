using Light.EventBus.Events;

namespace EventBusSample.Common;

public abstract record IntegrationEvent : IIntegrationEvent
{
    protected IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }
}

public abstract record EventBase : IntegrationEvent
{
    public int Value = 1000;
}
