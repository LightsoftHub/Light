using System;

namespace Light.EventBus.Events
{
    public interface IIntegrationEvent
    {
        Guid Id { get; }

        DateTime CreationDate { get; }
    }
}
