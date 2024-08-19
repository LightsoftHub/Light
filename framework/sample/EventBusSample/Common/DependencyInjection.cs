using Light.AspNetCore.Modules;
using MassTransit;

namespace EventBusSample.Common
{
    public class SampleModuleConsumers : ModuleConsumer
    {
        public override IBusRegistrationConfigurator AddComsumers(
            IBusRegistrationConfigurator configurator)
        {
            configurator.AddConsumer<ColorChangedConsumer, ColorChangedConsumerDefinition>();

            Console.WriteLine($"Module {GetType().Name} injected");

            return configurator;
        }
    }
}
