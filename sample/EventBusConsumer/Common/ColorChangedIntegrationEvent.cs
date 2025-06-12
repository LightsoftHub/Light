using Light.EventBus.Events;

namespace EventBusConsumer.Common
{
    [BindingName("color-value-changed")]
    //[MessageUrn("color-value-changed")]
    public record ColorChangedIntegrationEvent : EventBase
    {
        public string OldColor { get; set; } = null!;

        public string NewColor { get; set; } = null!;

        public DateTime ChangeOn { get; set; }
    }
}
