using Light.AspNetCore.Modularity;
using MassTransit;

namespace EventBusSample.Common
{
    public class SampleModuleConsumers : ModuleConsumer
    {
        public override void AddConsumers(IBusRegistrationConfigurator configurator)
        {
            configurator.AddConsumer<ColorChangedConsumer, ColorChangedConsumerDefinition>();

            Console.WriteLine($"Module {GetType().Name} injected");
        }
    }
}
