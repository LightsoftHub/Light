using Light.EventBus.Events;

namespace EventBusSample.Common
{
    [BindingName("color-value-changed")]
    public record ColorChangedIntegrationEvent : EventBase
    {
        public string OldColor { get; set; } = null!;

        public string NewColor { get; set; } = null!;

        public DateTime ChangeOn { get; set; }
    }
}
